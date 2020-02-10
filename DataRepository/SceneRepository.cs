using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryAI.Shared;

namespace DataRepository
{
    public class SceneRepository : IRepository
    {
        #region Instance
        private static SceneRepository instance;
        public static SceneRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new SceneRepository();
                return instance;
            }
        }
        public static SceneRepository CreateNew(ExecutionContext executionContext, string toReplace)
        {
            if (string.IsNullOrEmpty(Instance.WorkingDirectory))
                Instance.InitializeRepo(executionContext, toReplace);
            if (Instance.Data == null)
                Instance.Data = Instance.Data.InitializeData(Instance.WorkingDirectory);
            return Instance;
        }
        #endregion
        public string WorkingDirectory { get; set; }
        PersistentData<Scene> Data { get; set; }

        public IEnumerable<Scene> GetAll()
        {
            if (Data == null || Data.Data == null)
                throw new RepositoryInitializatonException();
            return Data.Data.Cast<Scene>();
        }
        public Scene GetByPK(Int64 pk)
        {
            if (Data == null)
                throw new RepositoryInitializatonException();
            return Data.Get(pk);
        }
        public IEnumerable<Scene> GetAllForStory(Int64 storyId)
        {
            List<Scene> results = new List<Scene>();
            foreach (KeyValuePair<Int64,string> kvp in Data.Data)
            {
                Scene scene = kvp.Value.Cast<Scene>();
                if (scene.StoryId == storyId)
                    results.Add(scene);
            }
            return results;
        }
        public IEnumerable<SceneTitle> GetSceneTitlesForStory(Int64 storyId)
        {
            return GetAllForStory(storyId).Select(s => new SceneTitle
            {
                PrimaryKey = s.PrimaryKey,
                StoryId = s.StoryId,
                Title = s.Title
            });
        }
        public IEnumerable<Scene> GetForStoryChoiceNull(Int64 storyId)
        {
            List<Scene> results = new List<Scene>();
            foreach (Scene scene in GetAllForStory(storyId))
            {
                if (!scene.ChoiceId.HasValue)
                    results.Add(scene);
            }
            return results;
        }
        public Scene AddOrUpdate(Scene Scene)
        {
            if (Data == null)
                throw new RepositoryInitializatonException();
            return Data.AddOrUpdate(Scene);
        }
    }
}
