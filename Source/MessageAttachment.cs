namespace SecMailClient{
	public class MessageAttachment
    {
        [NonSerialized]
        public long MaxSize = 1024 * 1024 * 64; // 64 mib

        public AttachmentType Type = AttachmentType.Raw;
        public string Name = "";
        public byte[] Data;

        public enum AttachmentType
        {
            Raw = 0x00,
            Text = 0x01,
            Image = 0x02,
            Video = 0x03,
            Gif = 0x04,
            Archive = 0x05,
            Executable = 0x06,
            Script = 0x07,
            Code = 0x08
        }

        public MessageAttachment()
        {

        }

        public MessageAttachment Create(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            if (new FileInfo(filePath).Length > MaxSize) return null;

            string[] tss = filePath.Split('\\');
            Name = tss[tss.Length - 1];
            Data = File.ReadAllBytes(filePath);
            string[] splits = filePath.Split('.');
            string ending = splits[splits.Length - 1].ToLower();
            if (splits.Length <= 1)
            {
                Type = AttachmentType.Raw;
            }
            else
            {
                if (ending == "bin") Type = AttachmentType.Raw;
                else if (ending == "txt") Type = AttachmentType.Text;
                else if (ending == "exe" || ending == "com" || ending == "cmd") Type = AttachmentType.Executable;
                else if (ending == "sh" || ending == "js" || ending == "py" || ending == "bat" || ending == "php" || ending == "lua"
                     || ending == "vbs") Type = AttachmentType.Script;
                else if (ending == "bmp" || ending == "png" || ending == "jpg" || ending == "jpeg") Type = AttachmentType.Image;
                else if (ending == "gif") Type = AttachmentType.Gif;
                else if (ending == "mp4" || ending == "mov" || ending == "avi") Type = AttachmentType.Video;
                else if (ending == "zip" || ending == "jar" || ending == "tar" || ending == "gz" || ending == "7z" || ending == "apk"
                     || ending == "cab" || ending == "rar") Type = AttachmentType.Archive;
                else if (ending == "cs" || ending == "cpp" || ending == "c" || ending == "java" || ending == "asm")
                    Type = AttachmentType.Code;
                else Type = AttachmentType.Raw;
            }

            return this;
        }
    }
}