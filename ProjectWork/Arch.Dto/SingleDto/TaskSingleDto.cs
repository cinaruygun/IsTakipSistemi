using Arch.Core;
using System.Collections.Generic;
namespace Arch.Dto.SingleDto
{
    public class TaskSingleDto
    {
        public Task Task { get; set; }
        public List<long> MediaIds { get; set; }
        public List<string> ContentTypes { get; set; }
        public List<string> FileNames { get; set; }
    }
}