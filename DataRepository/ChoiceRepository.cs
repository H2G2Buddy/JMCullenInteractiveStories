using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryAI.Shared;

namespace DataRepository
{

    public class ChoiceRepository : IRepository
    {
        #region Instance
        private static ChoiceRepository instance;
        public static ChoiceRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new ChoiceRepository();
                return instance;
            }
        }
        public static ChoiceRepository CreateNew(ExecutionContext executionContext, string toReplace)
        {
            if (string.IsNullOrEmpty(Instance.WorkingDirectory))
                Instance.InitializeRepo(executionContext, toReplace);
            if (Instance.Data == null)
                Instance.Data = Instance.Data.InitializeData(Instance.WorkingDirectory);
            return Instance;
        }
        #endregion
        public string WorkingDirectory { get; set; }
        PersistentData<Choice> Data { get; set; }

        public IEnumerable<Choice> GetAll()
        {
            if (Data == null || Data.Data == null)
                throw new RepositoryInitializatonException();
            return Data.Data.Cast<Choice>();
        }
        public Choice GetByPK(Int64 pk)
        {
            if (Data == null)
                throw new RepositoryInitializatonException();
            return Data.Get(pk);
        }
        public Choice AddOrUpdate(Choice choice)
        {
            if (Data == null)
                throw new RepositoryInitializatonException();
            return Data.AddOrUpdate(choice);
        }
    }
}
