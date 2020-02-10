using DataRepository;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Text;
using StoryAI.Shared;

namespace JMCullenInteractiveStories.DataRepository
{
    public class ActivityRepository : IRepository
    {
        #region Instance
        private static ActivityRepository instance;
        public static ActivityRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new ActivityRepository();
                return instance;
            }
        }
        public static ActivityRepository CreateNew(ExecutionContext executionContext, string toReplace)
        {
            if (string.IsNullOrEmpty(Instance.WorkingDirectory))
                Instance.InitializeRepo(executionContext, toReplace);
            if (Instance.Data == null)
                Instance.Data = Instance.Data.InitializeData(Instance.WorkingDirectory);
            return Instance;
        }
        #endregion
        public string WorkingDirectory { get; set; }
        PersistentData<ReaderActivity> Data { get; set; }

        public IEnumerable<ReaderActivity> GetAll()
        {
            if (Data == null || Data.Data == null)
                throw new RepositoryInitializatonException();
            return Data.Data.Cast<ReaderActivity>();
        }
        public ReaderActivity GetByPK(Int64 pk)
        {
            if (Data == null)
                throw new RepositoryInitializatonException();
            return Data.Get(pk);
        }
        public ReaderActivity AddOrUpdate(ReaderActivity Activity)
        {
            if (Data == null)
                throw new RepositoryInitializatonException();
            return Data.AddOrUpdate(Activity);
        }
    }
}
