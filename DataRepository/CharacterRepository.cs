using Microsoft.Azure.WebJobs;
using StoryAI.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataRepository
{
    public class CharacterRepository : IRepository
    {
        #region Instance
        private static CharacterRepository instance;
        public static CharacterRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new CharacterRepository();
                return instance;
            }
        }
        public static CharacterRepository CreateNew(ExecutionContext executionContext, string toReplace)
        {
            if (string.IsNullOrEmpty(Instance.WorkingDirectory))
                Instance.InitializeRepo(executionContext, toReplace);
            if (Instance.Data == null)
                Instance.Data = Instance.Data.InitializeData(Instance.WorkingDirectory);
            return Instance;
        }
        #endregion

        public string WorkingDirectory { get; set; }
        PersistentData<Character> Data { get; set; }

        public IEnumerable<Character> GetAllForStory(Int64 storyId)
        {
            List<Character> results = new List<Character>();
            foreach (KeyValuePair<Int64, string> kvp in Data.Data)
            {
                Character scene = kvp.Value.Cast<Character>();
                if (scene.StoryId == storyId)
                    results.Add(scene);
            }
            return results;
        }

        public Character AddOrUpdate(Character character)
        {
            if (Data == null)
                throw new RepositoryInitializatonException();
            return Data.AddOrUpdate(character);
        }
    }
}
