namespace EndlessShapes2.Polygon
{
    public static class StructureBlockGUID_Memory
    {
        private static StructureBlockGUID[] SBGUIDs = new StructureBlockGUID[]
        {
            new StructureBlockGUID(
                "2d519ca8-1f12-4a8e-9340-aa6648b5e799",
                "174b5b41-b70e-485d-b00a-a61cc9826b2c",
                "573f6de1-1379-49f8-8342-588bd81a50b7",
                "f8cd4d8b-c7ef-434e-a404-04423fb5fcae"),
            new StructureBlockGUID(
                "3cc75979-18ac-46c4-9a5b-25b327d99410",
                "911fe222-f9b2-4892-9cd6-8b154d55b2aa",
                "f1a3f1bd-b5f7-43bf-9b72-c7a060c24f73",
                "04019f87-c371-4574-acfe-b7086557eaba"),
            new StructureBlockGUID(
                "9a0ae372-beb4-4009-b14e-36ed0715af73",
                "bdafa446-f615-49cb-94f3-d7652dde6cec",
                "0b73f42f-ff32-4654-8857-aa13413bff33",
                "0de95539-4751-4355-88bd-156f17b5f64a"),
            new StructureBlockGUID(
                "ab699540-efc8-4592-bc97-204f6a874b3a",
                "5548037e-8428-43f8-bcb6-d730dbcd0a79",
                "ea2f8200-a920-40fc-9715-d0f66ae5f492",
                "ad00935b-e95c-4345-8ea7-646846bc16db"),
            new StructureBlockGUID(
                "710ee212-563b-42f8-acd1-57515479524d",
                "11fcac17-e3b9-47d5-aeb8-2224d86b2f1d",
                "0630b5e3-d51b-4441-8533-c054e018ee64",
                "e62d5f04-5c7a-4524-b97d-d60504babb2f"),
            new StructureBlockGUID(
                "e71e6f97-fbe8-4bf5-9645-d15179ba0c17",
                "df61d4c4-a514-4f23-baab-4da8fce066a3",
                "6a20f299-6c3e-4406-a859-157075aab08d",
                "995d25a2-7237-4cd2-b763-7eb3b3f7e1e7"),
            new StructureBlockGUID(
                "0c03433e-8947-4e7d-9dec-793526fe06d1",
                "78b81c0a-44df-4c24-b2a5-5d273737da60",
                "a0945b5c-2f1e-45ce-95fb-721e5657afa7",
                "60b279e2-9c1e-409f-8248-568039537baa"),
            new StructureBlockGUID(
                "6c0bab88-aa88-4825-9cf5-55df36aa12b8",
                "552d8144-11c0-46e6-8607-927f825b18be",
                "f9b1d3e4-c4c8-47eb-9547-b4bf3f3ba730",
                "4da4057d-1f5a-4d82-97dd-10502ae2bb80")
        };

        public static StructureBlockGUID GetSBGUID(StructureBlockType structureBlockType)
        {
            return SBGUIDs[(int)structureBlockType];
        }
    }
}
