using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using System;
using UnityEngine;

namespace EndlessShapes2.Polygon
{
    public class MimicAndDecorationCommonData
    {
        public static void Copy(MimicAndDecorationCommonData source, MimicAndDecorationCommonData destination)
        {
            destination.MeshGuid = source.MeshGuid;
            destination.Positioning = source.Positioning;
            destination.Scaling = source.Scaling;
            destination.Orientation = source.Orientation;
            destination.ColorIndex = source.ColorIndex;
        }



        private MAD_DataType _dataType = MAD_DataType.None;

        private object _obj;

        private Guid _meshGuid = new Guid();

        private Vector3 _positioning = Vector3.zero;

        private Vector3 _scaling = Vector3.one;

        private Vector3 _orientation = Vector3.zero;

        private int _colorIndex = 0;

        public Guid MeshGuid
        {
            get
            {
                switch (_dataType)
                {
                    case MAD_DataType.None:
                        return _meshGuid;
                    case MAD_DataType.Mimic:
                        return ((Mimic)_obj).Data.MeshGuid.Us;
                    case MAD_DataType.Decoration:
                        return ((Decoration)_obj).MeshGuid.Us;
                    default:
                        return default;
                }
            }
            set
            {
                switch (_dataType)
                {
                    case MAD_DataType.None:
                        _meshGuid = value;
                        break;
                    case MAD_DataType.Mimic:
                        ((Mimic)_obj).Data.MeshGuid.Us = value;
                        break;
                    case MAD_DataType.Decoration:
                        ((Decoration)_obj).MeshGuid.Us = value;
                        break;
                }
            }
        }

        public Vector3 Positioning
        {
            get
            {
                switch (_dataType)
                {
                    case MAD_DataType.None:
                        return _positioning;
                    case MAD_DataType.Mimic:
                        return ((Mimic)_obj).Data.Positioning.Us;
                    case MAD_DataType.Decoration:
                        return ((Decoration)_obj).Positioning.Us;
                    default:
                        return default;
                }
            }
            set
            {
                switch (_dataType)
                {
                    case MAD_DataType.None:
                        _positioning = value;
                        break;
                    case MAD_DataType.Mimic:
                        ((Mimic)_obj).Data.Positioning.Us = value;
                        break;
                    case MAD_DataType.Decoration:
                        ((Decoration)_obj).Positioning.Us = value;
                        break;
                }
            }
        }

        public Vector3 Scaling
        {
            get
            {
                switch (_dataType)
                {
                    case MAD_DataType.None:
                        return _scaling;
                    case MAD_DataType.Mimic:
                        return ((Mimic)_obj).Data.Scaling.Us;
                    case MAD_DataType.Decoration:
                        return ((Decoration)_obj).Scaling.Us;
                    default:
                        return default;
                }
            }
            set
            {
                switch (_dataType)
                {
                    case MAD_DataType.None:
                        _scaling = value;
                        break;
                    case MAD_DataType.Mimic:
                        ((Mimic)_obj).Data.Scaling.Us = value;
                        break;
                    case MAD_DataType.Decoration:
                        ((Decoration)_obj).Scaling.Us = value;
                        break;
                }
            }
        }

        public Vector3 Orientation
        {
            get
            {
                switch (_dataType)
                {
                    case MAD_DataType.None:
                        return _orientation;
                    case MAD_DataType.Mimic:
                        return ((Mimic)_obj).Data.Orientation.Us;
                    case MAD_DataType.Decoration:
                        return ((Decoration)_obj).Orientation.Us;
                    default:
                        return default;
                }
            }
            set
            {
                switch (_dataType)
                {
                    case MAD_DataType.None:
                        _orientation = value;
                        break;
                    case MAD_DataType.Mimic:
                        ((Mimic)_obj).Data.Orientation.Us = value;
                        break;
                    case MAD_DataType.Decoration:
                        ((Decoration)_obj).Orientation.Us = value;
                        break;
                }
            }
        }

        public int ColorIndex
        {
            get
            {
                switch (_dataType)
                {
                    case MAD_DataType.None:
                        return _colorIndex;
                    case MAD_DataType.Mimic:
                        return ((Mimic)_obj).color;
                    case MAD_DataType.Decoration:
                        return ((Decoration)_obj).Color.Us;
                    default:
                        return default;
                }
            }
            set
            {
                switch (_dataType)
                {
                    case MAD_DataType.None:
                        _colorIndex = value;
                        break;
                    case MAD_DataType.Mimic:
                        ((Mimic)_obj).SetColor(value);
                        break;
                    case MAD_DataType.Decoration:
                        ((Decoration)_obj).Color.Us = value;
                        break;
                }
            }
        }



        public float PositioningX
        {
            get
            {
                return Positioning.x;
            }
            set
            {
                Vector3 v = Positioning;
                v.x = value;
                Positioning = v;
            }
        }

        public float PositioningY
        {
            get
            {
                return Positioning.y;
            }
            set
            {
                Vector3 v = Positioning;
                v.y = value;
                Positioning = v;
            }
        }

        public float PositioningZ
        {
            get
            {
                return Positioning.z;
            }
            set
            {
                Vector3 v = Positioning;
                v.z = value;
                Positioning = v;
            }
        }

        public float ScalingX
        {
            get
            {
                return Scaling.x;
            }
            set
            {
                Vector3 v = Scaling;
                v.x = value;
                Scaling = v;
            }
        }

        public float ScalingY
        {
            get
            {
                return Scaling.y;
            }
            set
            {
                Vector3 v = Scaling;
                v.y = value;
                Scaling = v;
            }
        }

        public float ScalingZ
        {
            get
            {
                return Scaling.z;
            }
            set
            {
                Vector3 v = Scaling;
                v.z = value;
                Scaling = v;
            }
        }

        public float OrientationX
        {
            get
            {
                return Orientation.x;
            }
            set
            {
                Vector3 v = Orientation;
                v.x = value;
                Orientation = v;
            }
        }

        public float OrientationY
        {
            get
            {
                return Orientation.y;
            }
            set
            {
                Vector3 v = Orientation;
                v.y = value;
                Orientation = v;
            }
        }

        public float OrientationZ
        {
            get
            {
                return Orientation.z;
            }
            set
            {
                Vector3 v = Orientation;
                v.z = value;
                Orientation = v;
            }
        }



        public MimicAndDecorationCommonData()
        {
        }

        public MimicAndDecorationCommonData(Mimic mimic)
        {
            _dataType = MAD_DataType.Mimic;
            _obj = mimic;
        }

        public MimicAndDecorationCommonData(Decoration decoration)
        {
            _dataType = MAD_DataType.Decoration;
            _obj = decoration;
        }

        private enum MAD_DataType
        {
            None,
            Mimic,
            Decoration
        }
    }
}
