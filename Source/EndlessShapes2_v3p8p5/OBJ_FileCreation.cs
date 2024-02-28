using BrilliantSkies.Common.CarriedObjects;
using BrilliantSkies.Common.ChunkCreators.Chunks;
using BrilliantSkies.Core.Constants;
using BrilliantSkies.Ftd.Constructs.Modules.All.Chunks;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EndlessShapes2
{
    public static class OBJ_FileCreation
    {
        private static int _outputCount;

        private static string _mtlFileName = "Materials.mtl";

        public static void Start(MainConstruct mainConstruct)
        {
            _outputCount = 0;

            string outPutFolderPath = Get.ProfilePaths.ProfileRootDir().Append(mainConstruct.GetBlueprintName()).ToString() + string.Format("-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
            string texFolderPath = Path.Combine(outPutFolderPath, "Textures");

            Directory.CreateDirectory(outPutFolderPath);
            Directory.CreateDirectory(texFolderPath);

            //TextureDefinition getTexDef(MaterialDefinition I) => Configured.i.Textures.Find(I.ColorTextureReference.Reference.Guid);
            IEnumerable<TextureDefinition> textureDefinitionList = Configured.i.Materials.Components
                .Select(I => Configured.i.Textures.Find(I.ColorTextureReference.Reference.Guid))
                .Where(I => I != null)
                .Distinct();

            StringBuilder sb = new StringBuilder();

            foreach (TextureDefinition textureDefinition in textureDefinitionList)
            {
                ModSource modSource = textureDefinition.Source;

                if (modSource != ModSource.File && modSource != ModSource.Resources) continue;

                byte[] encodeResult = null;

                try
                {
                    encodeResult = ForcedEncodeToJPG(textureDefinition.Texture.GetTexture());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                if (encodeResult != null)
                {
                    string texName = TexNameGenerate(textureDefinition);

                    File.WriteAllBytes(Path.Combine(texFolderPath, texName + ".jpg"), encodeResult);

                    Console.WriteLine("newmtl " + texName);
                    Console.WriteLine("map_Kd " + Path.Combine("Textures", texName + ".jpg"));
                    Console.WriteLine(string.Empty);

                    sb.Append("newmtl " + texName + "\n");
                    sb.Append("map_Kd " + Path.Combine("Textures", texName + ".jpg") + "\n");
                    sb.Append("\n");
                }
            }

            using (StreamWriter sw = new StreamWriter(Path.Combine(outPutFolderPath, _mtlFileName)))
            {
                sw.Write(sb.ToString());
            }



            List<AllConstruct> allConstructList = new List<AllConstruct>();
            mainConstruct.AllBasicsRestricted.GetAllConstructsBelowUsAndIncludingUs(allConstructList);

            foreach (AllConstruct allConstruct in allConstructList)
            {
                if (!(allConstruct.Chunks is ConstructableMeshMerger)) continue;

                Dictionary<MaterialDefinition, List<Mesh>> meshListDictionary = new Dictionary<MaterialDefinition, List<Mesh>>();

                foreach (ICarriedObjectReference iCOR in allConstruct.CarriedObjects.Objects)
                {
                    MeshRenderer meshRenderer = iCOR.ObjectItself.GetComponent<MeshRenderer>();
                    MeshFilter meshFilter = iCOR.ObjectItself.GetComponent<MeshFilter>();

                    if (meshRenderer == null || meshFilter == null) continue;

                    KeyValuePair<Guid, MaterialDefinition> item = Configured.i.Materials.DictionaryOfComponents.FirstOrDefault(d => d.Value.Material == meshRenderer.sharedMaterial);

                    if (item.Equals(default(KeyValuePair<Guid, MaterialDefinition>))) continue;

                    if (!meshListDictionary.ContainsKey(item.Value))
                    {
                        meshListDictionary.Add(item.Value, new List<Mesh>());
                    }

                    Mesh newMesh = UnityEngine.Object.Instantiate(meshFilter.sharedMesh);
                    IEnumerable<Vector3> newVertexList = newMesh.vertices.Select(d => meshFilter.transform.localToWorldMatrix.MultiplyPoint(d));
                    newVertexList = newVertexList.Select(d => allConstruct.myTransform.worldToLocalMatrix.MultiplyPoint(d));
                    newMesh.SetVertices(newVertexList.ToList());

                    meshListDictionary[item.Value].Add(newMesh);
                }

                ConstructableMeshMerger meshMerger = allConstruct.Chunks as ConstructableMeshMerger;
                Console.WriteLine("AllBlockVertex Count : " + meshMerger.VertexCount);

                foreach (KeyValuePair<int, List<ChunkMesh>> chunkMeshListDictionary in meshMerger.D)
                {
                    bool found;
                    MaterialDefinition materialDefinition = Configured.i.Materials.FindUsingTheRuntimeId(chunkMeshListDictionary.Key, out found);

                    bool flag_0 = chunkMeshListDictionary.Value.Count == 0;
                    bool flag_1 = chunkMeshListDictionary.Value.All(d => d.VertCount == 0);

                    if (!found || flag_0 || flag_1) continue;

                    if (!meshListDictionary.ContainsKey(materialDefinition))
                    {
                        meshListDictionary.Add(materialDefinition, new List<Mesh>());
                    }

                    meshListDictionary[materialDefinition].AddRange(chunkMeshListDictionary.Value.Select(d => d.GetMesh()));
                }



                int subConstructIndex = allConstruct.PersistentSubConstructIndex;
                bool isSubConstruct = subConstructIndex != -1;

                Vector3 localPosition = Vector3.zero;
                Quaternion localRotation = Quaternion.identity;

                if (isSubConstruct)
                {
                    MainConstruct mc = allConstruct.Main;
                    localPosition = mc.SafeGlobalToLocal(allConstruct.SafePosition);
                    localRotation = mc.SafeGlobalRotationToLocalRotation(allConstruct.SafeRotation);
                }

                List<Mesh> meshList = new List<Mesh>();

                foreach (KeyValuePair<MaterialDefinition, List<Mesh>> meshListDictionaryData in meshListDictionary)
                {
                    List<Vector3> vertices = new List<Vector3>();
                    List<Vector3> normals = new List<Vector3>();
                    List<Vector2> uv = new List<Vector2>();
                    List<int> triangles = new List<int>();
                    int loadVertexCount = 0;

                    foreach (Mesh mesh in meshListDictionaryData.Value)
                    {
                        vertices.AddRange(mesh.vertices);
                        normals.AddRange(mesh.normals);
                        uv.AddRange(mesh.uv);
                        triangles.AddRange(mesh.triangles.Select(d => d + loadVertexCount));
                        loadVertexCount += mesh.vertexCount;
                    }

                    if (isSubConstruct)
                    {
                        vertices = vertices.Select(I => localRotation * I + localPosition).ToList();
                    }

                    TextureDefinition textureDefinition = Configured.i.Textures.Find(meshListDictionaryData.Key.ColorTextureReference.Reference.Guid);

                    Mesh newMesh = new Mesh
                    {
                        indexFormat = UnityEngine.Rendering.IndexFormat.UInt32,
                        name = TexNameGenerate(textureDefinition)
                    };

                    newMesh.SetVertices(vertices);
                    newMesh.SetNormals(normals);
                    newMesh.SetUVs(0, uv);
                    newMesh.SetTriangles(triangles, 0);

                    FlipHorizontal(newMesh);
                    meshList.Add(newMesh);

                    Console.WriteLine("vertexCount : " + newMesh.vertices.Length);
                }



                string fileName = $"Test ({_outputCount++})";

                if (subConstructIndex == -1)
                {
                    fileName = "MainConstruct";
                }
                else
                {
                    fileName = $"SubConstruct_{subConstructIndex}";
                }

                MeshToFile(meshList, Path.Combine(outPutFolderPath, fileName + ".obj"));
            }
        }

        private static string SeparatorClean(string path)
        {
            char directorySeparatorChar = Path.DirectorySeparatorChar;
            path = path.Replace('\\', directorySeparatorChar).Replace('/', directorySeparatorChar);
            return path;
        }

        private static void FlipHorizontal(Mesh mesh)
        {
            IEnumerable<Vector3> newVertices = mesh.vertices.Select(d => new Vector3(-d.x, d.y, d.z));
            mesh.SetVertices(newVertices.ToList());

            int[] triangles = mesh.triangles;

            for (int index = 0; index < triangles.Length; index += 3)
            {
                int temp = triangles[index];
                triangles[index] = triangles[index + 1];
                triangles[index + 1] = temp;
            }

            mesh.SetTriangles(triangles, 0);

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }

        private static string MeshToString(List<Mesh> meshList)
        {
            StringBuilder sb = new StringBuilder();
            int importVertexCount = 1;

            sb.Append("mtllib " + _mtlFileName + "\n");

            foreach (Mesh mesh in meshList)
            {
                sb.Append("\n");
                sb.Append("g " + mesh.name + "\n");
                sb.Append("usemtl " + mesh.name + "\n");

                foreach (Vector3 v in mesh.vertices)
                {
                    sb.Append($"v {v.x} {v.y} {v.z}" + "\n");
                }

                /*
                foreach (Vector3 v in mesh.normals)
                {
                    sb.Append($"vn {v.x} {v.y} {v.z}" + "\n");
                }
                */

                foreach (Vector3 v in mesh.uv)
                {
                    sb.Append($"vt {v.x} {v.y}" + "\n");
                }

                for (int material = 0; material < mesh.subMeshCount; material++)
                {
                    int[] triangles = mesh.GetTriangles(material);

                    for (int i = 0; i < triangles.Length; i += 3)
                    {
                        /*
                        sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}" + "\n",
                            triangles[i] + importVertexCount, triangles[i + 1] + importVertexCount, triangles[i + 2] + importVertexCount));
                        */
                        sb.Append(string.Format("f {0}/{0} {1}/{1} {2}/{2}" + "\n",
                            triangles[i] + importVertexCount, triangles[i + 1] + importVertexCount, triangles[i + 2] + importVertexCount));
                    }
                }

                importVertexCount += mesh.vertexCount;
            }

            return sb.ToString();
        }

        private static void MeshToFile(List<Mesh> m, string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(MeshToString(m));
            }
        }

        private static byte[] ForcedEncodeToJPG(Texture2D texture2D)
        {
            RenderTexture renderTexture = RenderTexture.GetTemporary(texture2D.width, texture2D.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(texture2D, renderTexture);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTexture;

            Texture2D copyTexture2D = new Texture2D(texture2D.width, texture2D.height);
            copyTexture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            copyTexture2D.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTexture);

            return copyTexture2D.EncodeToJPG();
        }

        private static string TexNameGenerate(TextureDefinition textureDefinition)
        {
            if (textureDefinition == null)
            {
                return "None";
            }
            else
            {
                string texName = SeparatorClean(textureDefinition.FilenameOrUrl);
                return "[" + textureDefinition.Source.ToString() + "]-[" + texName.Replace(Path.DirectorySeparatorChar.ToString(), "]-[") + "]";
            }
        }
    }
}
