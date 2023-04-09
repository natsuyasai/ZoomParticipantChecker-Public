using System.Collections.Generic;

namespace ZoomParticipantChecker.Model
{
    /// <summary>
    /// プリセット情報
    /// </summary>
    public class PresetInfo
    {
        /// <summary>
        /// 識別子 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// プリセット名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// プリセットデータ
        /// </summary>
        public IEnumerable<string> Data { get; set; }
    }
}
