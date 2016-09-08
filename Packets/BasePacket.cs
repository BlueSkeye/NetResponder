using System;
using System.Collections.Generic;
using System.Text;

namespace NetResponder.Packets
{
    // Packet class handling all packet generation (see odict.py).
    internal abstract class BasePacket
    {
        protected BasePacket(byte[] defaultValue)
        {
            _data = defaultValue;
            //fields = new SortedList<string, string>();
            //fields.Add("data", string.Empty);
            ////     self.fields = OrderedDict(self.__class__.fields)
            //foreach(KeyValuePair<string, byte[]> pair in EnumerateFieldsInOrder()) {
            //    _valueIndexPerFieldName.Add(pair.Key, _fieldValues.Count);
            //    //if callable(v):
            //    //  self.fields[k] = v(self.fields[k])
            //    //else:
            //    //  self.fields[k] = v
            //    _fieldValues.Add(pair.Value);
            //}
        }

        internal byte[] RawData
        {
            get {
                byte[] result = new byte[_data.Length];
                Buffer.BlockCopy(_data, 0, result, 0, result.Length);
                return result;
            }
        }

        protected static void Append(List<byte> builder, out ItemDescriptor descriptor, byte[] data)
        {
            descriptor = new ItemDescriptor(builder.Count, data.Length);
            builder.AddRange(data);
        }

        protected byte[] GetData(ItemDescriptor descriptor)
        {
            byte[] result = new byte[descriptor.Length];
            Buffer.BlockCopy(_data, descriptor.Offset, result, 0, descriptor.Length);
            return result;
        }

        protected void SetData(ItemDescriptor descriptor, byte[] value)
        {
            if (descriptor.Length != value.Length)
            {
                throw new ApplicationException();
            }
            Buffer.BlockCopy(value, 0, _data, descriptor.Offset, descriptor.Length);
        }

        internal static byte[] Concatenate(int reservedLeadingSpace, BasePacket header,
            params BasePacket[] others)
        {
            List<byte> result = new List<byte>();
            while(0 < reservedLeadingSpace--) { result.Add(0x00); }
            result.AddRange(header._data);
            foreach(BasePacket other in others) { result.AddRange(other._data); }
            return result.ToArray();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach(KeyValuePair<string, string> pair in fields) {
                if (0 < builder.Length) { builder.Append("; "); }
                builder.AppendFormat("{0}={1}", pair.Key, pair.Value);
            }
            return builder.ToString();
        }

        private byte[] _data;
        private List<byte[]> _fieldValues = new List<byte[]>();
        private Dictionary<string, int> _valueIndexPerFieldName = new Dictionary<string, int>();
        private SortedList<string, string> fields;

        protected struct ItemDescriptor
        {
            internal ItemDescriptor(int offset, int length)
            {
                Offset = offset;
                Length = length;
            }

            internal int Offset { get; private set; }
            internal int Length { get; private set; }
        }
    }
}