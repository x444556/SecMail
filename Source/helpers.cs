public static string Storage(ulong bytes)
{
    if (bytes >= 1000000000000) return Math.Round(bytes / 1000000000000.0, 2).ToString("0.00") + " TB";
    else if (bytes >= 1000000000) return Math.Round(bytes / 1000000000.0, 2).ToString("0.00") + " GB";
    else if (bytes >= 1000000) return Math.Round(bytes / 1000000.0, 2).ToString("0.00") + " MB";
    else if (bytes >= 1000) return Math.Round(bytes / 1000.0, 2).ToString("0.00") + " KB";
    else return bytes + "  B";
}