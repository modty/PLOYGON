namespace Tools
{
    public class UidTool
    {
        private static UidTool _instance;
        private static long uid;
        private UidTool()
        {
        }

        public long RegistUid()
        {
            return uid++;
        }
        public static UidTool Instance => _instance ??= new UidTool();
    }
}