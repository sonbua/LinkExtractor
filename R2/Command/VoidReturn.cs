namespace R2
{
    public sealed class VoidReturn
    {
        static VoidReturn()
        {
        }

        private VoidReturn()
        {
        }

        public static VoidReturn Instance { get; } = new VoidReturn();
    }
}