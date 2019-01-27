using IC.DotNet.Interview.Logic.Models;
using System.Collections.Generic;

namespace IC.DotNet.Interview.Logic.BL
{
    public interface ITaskLogic
    {
        IEnumerable<TaskViewModel> Get();
        TaskViewModel Get(string id);
        bool Add(TaskViewModel task);
        bool Edit(TaskViewModel task);
        bool Delete(string id);
        IEnumerable<CommentViewModel> GetTaskComments(string taskId);
        bool AddTaskComment(string taskId, CommentViewModel comment);
    }
}
