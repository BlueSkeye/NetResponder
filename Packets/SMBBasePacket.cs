
namespace NetResponder.Packets
{
    internal abstract class SMBBasePacket : BasePacket
    {
        protected SMBBasePacket(byte[] defaultValue)
            : base(defaultValue)
        {
            return;
        }

        internal byte[] Build(SMBHeader header)
        {
            return base.Build(sizeof(int), header, this);
        }

        internal abstract void Calculate();

        internal byte[] CalculateAndBuild(SMBHeader header)
        {
            Calculate();
            return Build(header);
        }
    }
}
