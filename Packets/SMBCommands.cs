using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetResponder.Packets
{
    internal enum SMBCommands : byte
    {
        CreateDirectory = 0x00, // CORE
        DeleteDirectory = 0x01, // CORE
        Open = 0x02, // CORE
        Create = 0x03, // CORE
        Close = 0x04, // CORE
        Flush = 0x05, // CORE
        Delete = 0x06, // CORE
        Rename = 0x07, // CORE
        QueryInformation = 0x08, // CORE
        SetInformation = 0x09, // CORE
        Read = 0x0A, // CORE
        Write = 0x0B, // CORE
        LockByteRange = 0x0C, // CORE
        UnlockByteRange = 0x0D, // CORE
        CreateTemporary = 0x0E, // CORE
        CreateNew = 0x0F, // CORE
        CheckDirectory = 0x10, // CORE
        ProcessExit = 0x11, // CORE
        Seek = 0x12, // CORE
        LockAndRead = 0x13, // CorePlus
        WriteAndUnlock = 0x14, // CorePlus
        // Unused 0x15 ... 0x19
        ReadRaw = 0x1A, // CorePlus
        ReadMultiplexed = 0x1B, // LANMAN1.0
        ReadMultiplexedSecondary = 0x1C, // LANMAN1.0
        WriteRaw = 0x1D, // CorePlus
        WriteMultiplexed = 0x1E, // LANMAN1.0
        WriteMultiplexedSecondary = 0x1F, // LANMAN1.0
        WriteComplete = 0x20, // LANMAN1.0
        QueryServer = 0x21, // Reserved, but not implemented.
        SetInformation2 = 0x22, // LANMAN1.0
        QueryInformation2 = 0x23, // LANMAN1.0
        LockMultipleByteRanges = 0x24, // LANMAN1.0
        Transaction = 0x25, // LANMAN1.0
        TransactionSecondary = 0x26, // LANMAN1.0
        IOControl = 0x27, // LANMAN1.0
        IOControlSecondary = 0x28, // LANMAN1.0
        Copy = 0x29, // LANMAN1.0
        Move = 0x2A, // LANMAN1.0
        Echo = 0x2B, // LANMAN1.0
        WriteANdClose = 0x2C, // LANMAN1.0
        OpenAndXChaining = 0x2D, // LANMAN1.0
        ReadWithAndXChaining = 0x2E, // LANMAN1.0
        WriteWithAndXChaining = 0x2F, // LANMAN1.0
        NewFileSize = 0x30, // Reserved, but not implemented.
        CloseANdTreeDisconnect = 0x31, // NT LANMAN
        Transaction2 = 0x32, // LANMAN1.2
        Transaction2Secondary = 0x33, // LANMAN1.2
        CloseSearch = 0x34, // LANMAN1.2
        SearchCloseNotify = 0x35, // LANMAN1.2
        // Unused 0x36 ... 0x5F
        // This range of codes was reserved for use by the "xenix1.1" dialect of
        // SMB.See[MSFT - XEXTNP]. [XOPEN-SMB] page 41 lists this range as
        // "Reserved for proprietary dialects."
        // Reserved 0x60 ... 0x6F
        ConnectTree = 0x70, // CORE
        DisconnectTree = 0x71, // CORE
        Negociate = 0x72, // CORE
        SessionSetupWithAndXChaining = 0x73, // LANMAN1.0
        LogoffWithAndXChaining = 0x74, // LANMAN1.2
        ConnectTreeWithAndXChaining = 0x75, // LANMAN1.0
        // Unused 0x76 ... 0x7D
        NegociateSecurityPackageWithAndXChaining = 0x7E, // LANMAN1.0
        // Unused 0x7F
        QueryInformationDisk = 0x80, // CORE
        Search = 0x81, // CORE
        Find = 0x82, // LANMAN1.0
        findUnique = 0x83,  // LANMAN1.0
        FindClose = 0x84, // LANMAN1.0
        // Unused 0x85 ...  0x9F
        NTFormatTransaction = 0xA0, // NT LANMAN
        NTFormatTransactionSecondary = 0xA1, // NT LANMAN
        NTCreateAndX = 0xA2, // NT LANMAN
        // Unused 0xA3
        NTCancel = 0xA4, // NT LANMAN
        NTRename = 0xA5, // NT LANMAN
        // Unused 0xA6 ... 0xBF
        OpenPrintFile = 0xC0, // CORE
        WritePrintFile = 0xC1, // CORE
        ClosePrintFile = 0xC2, // CORE
        GetPrintQueue = 0xC3, // CORE
        // Unused 0xC4 ... 0xCF
        // This range is reserved for use by the SMB Messenger Service.
        // See[MS - MSRP], and section 6 of[SMB - CORE].
        // Reserved 0xD0 ... 0xD7
        ReadBulk = 0xD8, // Reserved, but not implemented.
        WriteBulk = 0xD9, // Reserved, but not implemented.
        WriteBulkData = 0x0A, // Reserved, but not implemented.
        // Unused 0xDB ... 0xFD
        Invalid = 0xFE, // LANMAN1.0
        NoAndXCommand = 0xFF,// LANMAN1.0
    }
}
