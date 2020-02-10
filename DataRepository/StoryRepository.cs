using Microsoft.Azure.WebJobs;
using StoryAI.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepository
{
    public class StoryRepository : IRepository
    {
        #region Instance
        private static StoryRepository instance;
        public static StoryRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new StoryRepository();
                return instance;
            }
        }
        public static StoryRepository CreateNew(ExecutionContext executionContext, string toReplace)
        {
            if (string.IsNullOrEmpty(Instance.WorkingDirectory))
                Instance.InitializeRepo(executionContext, toReplace);
            if (Instance.Data == null)
                Instance.Data = Instance.Data.InitializeData(Instance.WorkingDirectory);
            return Instance;
        }
        #endregion
        public string WorkingDirectory { get; set; }
        PersistentData<Story> Data { get; set; }

        public IEnumerable<Story> GetAll()
        {
            if (Data == null || Data.Data == null)
                throw new RepositoryInitializatonException();
            return Data.Data.Cast<Story>();
        }
        public Story GetByPK(Int64 pk)
        {
            if (Data == null)
                throw new RepositoryInitializatonException();
            return Data.Get(pk);
        }
        public Story AddOrUpdate(Story Story)
        {
            if (Data == null)
                throw new RepositoryInitializatonException();
            return Data.AddOrUpdate(Story);
        }
    }
}
