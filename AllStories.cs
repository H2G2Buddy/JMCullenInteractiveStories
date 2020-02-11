using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using DataRepository;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using StoryAI.Shared;

namespace JMCullenInteractiveStories
{
    public static class GetAllStories
    {
        [FunctionName("StoryList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            IEnumerable<Story> results = StoryRepository.CreateNew(executionContext, "StoryList").GetAll();
            if (results == null || results.Count() == 0)
                return (ActionResult)new OkObjectResult(new RequestResponse<Story>(new ErrorReadingDataFile(typeof(Story).Name)));
            else
                return (ActionResult)new OkObjectResult(new RequestResponse<Story>(results));
        }
    }

    public class UpdateStory
    {
        [FunctionName("StoryUpdate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                if (string.IsNullOrEmpty(requestBody))
                    return (ActionResult)new OkObjectResult(new RequestResponse<Story>(new RequestBodyEmptyException()));
                Story data = JsonConvert.DeserializeObject<Story>(requestBody);

                Story results = StoryRepository.CreateNew(executionContext, "StoryUpdate").AddOrUpdate(data);
                if (results == null || results.PrimaryKey == 0)
                    return (ActionResult)new OkObjectResult(new InsertRecordException(data.Title));
                else
                    return (ActionResult)new OkObjectResult(new RequestResponse<Story>(results));
            }
            catch (Exception e)
            {
                return (ActionResult)new BadRequestObjectResult(new UpdateRecordException(typeof(Scene).Name));
            }
        }
    }

    public static class GetStorySceneTitles
    {
        [FunctionName("SceneTitlesList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            Story data = await req.GetFromBody<Story>();
            var results = SceneRepository.CreateNew(executionContext, "SceneTitlesList").GetSceneTitlesForStory(data.PrimaryKey);
            return (ActionResult)new OkObjectResult(new RequestResponse<SceneTitle>(results));
        }
    }

    #region Scenes
    public static class GetSceneByPK
    {
        [FunctionName("Scene")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            Scene data = await req.GetFromBody<Scene>();
            Scene Scene = SceneRepository.CreateNew(executionContext, "Scene").GetByPK(data.PrimaryKey);
            if (Scene == null)
                return (ActionResult)new OkObjectResult(new RequestResponse<Scene>(new PrimaryKeyNotFoundException(data.PrimaryKey)));
            else
                return (ActionResult)new OkObjectResult(new RequestResponse<Scene>(Scene));
        }
    }

    public static class GetSceneByStoryId
    {
        [FunctionName("StoryScenes")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            Scene data = await req.GetFromBody<Scene>();
            IEnumerable<Scene> Scenes = SceneRepository.CreateNew(executionContext, "Scene").GetAllForStory(data.PrimaryKey);
            if (Scenes == null)
                return (ActionResult)new OkObjectResult(new RequestResponse<Scene>(new PrimaryKeyNotFoundException(data.PrimaryKey)));
            else
                return (ActionResult)new OkObjectResult(new RequestResponse<Scene>(Scenes));
        }
    }

    //public static class GetStoryScenesChoiceNull
    //{
    //    [FunctionName("MissingChoiceList")]
    //    public static async Task<IActionResult> Run(
    //        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
    //        ExecutionContext executionContext)
    //    {
    //        string primaryKey = req.Query["storyId"];
    //        Int64 storyId = 0;
    //        if (!Int64.TryParse(primaryKey, out storyId))
    //            return (ActionResult)new OkObjectResult(new RequestResponse<Scene>(new PrimaryKeyNotFoundException(primaryKey)));

    //        IEnumerable<Scene> results = SceneRepository.CreateNew(executionContext, "MissingChoiceList").GetForStoryChoiceNull(storyId);
    //        return (ActionResult)new OkObjectResult(new RequestResponse<Scene>(results));
    //    }
    //}

    public class UpdateScene
    {
        [FunctionName("SceneUpdate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                if (string.IsNullOrEmpty(requestBody))
                    return (ActionResult)new OkObjectResult(new RequestResponse<Scene>(new RequestBodyEmptyException()));
                Scene data = JsonConvert.DeserializeObject<Scene>(requestBody);

                Scene results = SceneRepository.CreateNew(executionContext, "SceneUpdate").AddOrUpdate(data);
                if (results == null)
                    return (ActionResult)new OkObjectResult(new InsertRecordException(data.Title));
                else
                    return (ActionResult)new OkObjectResult(new RequestResponse<Scene>(results));

            }
            catch (Exception e)
            {
                return (ActionResult)new BadRequestObjectResult(new UpdateRecordException(typeof(Scene).Name));
            }
        }
    }
    #endregion

    #region Choices
    public static class GetAllChoices
    {
        [FunctionName("ChoicesList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            IEnumerable<Choice> results = ChoiceRepository.CreateNew(executionContext, "ChoicesList").GetAll();
            if (results == null || results.Count() == 0)
                return (ActionResult)new OkObjectResult(new RequestResponse<Choice>(new ErrorReadingDataFile(typeof(Choice).Name)));
            else
                return (ActionResult)new OkObjectResult(new RequestResponse<Choice>(results));
        }
    }

    public static class GetChoiceByPK
    {
        [FunctionName("Choice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            Choice data = await req.GetFromBody<Choice>();
            Choice choice = ChoiceRepository.CreateNew(executionContext, "Choice").GetByPK(data.PrimaryKey);
            if (choice == null)
                return (ActionResult)new OkObjectResult(new RequestResponse<Choice>(new PrimaryKeyNotFoundException(data.PrimaryKey)));
            else
                return (ActionResult)new OkObjectResult(new RequestResponse<Choice>(choice));
        }
    }

    public class UpdateChoice
    {
        [FunctionName("ChoiceUpdate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                if (string.IsNullOrEmpty(requestBody))
                    return (ActionResult)new OkObjectResult(new RequestResponse<Choice>(new RequestBodyEmptyException()));
                Choice data = JsonConvert.DeserializeObject<Choice>(requestBody);

                Choice results = ChoiceRepository.CreateNew(executionContext, "ChoiceUpdate").AddOrUpdate(data);
                if (results == null || results.PrimaryKey == 0)
                    return (ActionResult)new OkObjectResult(new InsertRecordException(data.Text));
                else
                    return (ActionResult)new OkObjectResult(new RequestResponse<Choice>(results));
            }
            catch (Exception e)
            {
                return (ActionResult)new BadRequestObjectResult(new UpdateRecordException(typeof(Scene).Name));
            }
        }
    }
    #endregion

    public static class GetStoryCharacters
    {
        [FunctionName("StoryCharacters")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            Story data = await req.GetFromBody<Story>();
            var results = CharacterRepository.CreateNew(executionContext, "StoryCharacters").GetAllForStory(data.PrimaryKey);
            return (ActionResult)new OkObjectResult(new RequestResponse<Character>(results));
        }
    }

    public class UpdateCharacter
    {
        [FunctionName("CharacterUpdate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                if (string.IsNullOrEmpty(requestBody))
                    return (ActionResult)new OkObjectResult(new RequestResponse<Character>(new RequestBodyEmptyException()));
                Character data = JsonConvert.DeserializeObject<Character>(requestBody);

                Character results = CharacterRepository.CreateNew(executionContext, "CharacterUpdate").AddOrUpdate(data);
                if (results == null || results.PrimaryKey == 0)
                    return (ActionResult)new OkObjectResult(new InsertRecordException(data.LastNameFirst));
                else
                    return (ActionResult)new OkObjectResult(new RequestResponse<Character>(results));
            }
            catch (Exception e)
            {
                return (ActionResult)new BadRequestObjectResult(new UpdateRecordException(typeof(Scene).Name));
            }
        }
    }

    public static class Helpers
    {
        public async static Task<T> GetFromBody<T>(this HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync().ConfigureAwait(true);
            if (string.IsNullOrEmpty(requestBody))
                throw new RequestBodyEmptyException();

            T data = JsonConvert.DeserializeObject<T>(requestBody);
            if (data.TryGetColumnValue<T>("PrimaryKey") <= 0)
                throw new PrimaryKeyNotFoundException(requestBody);

            return data;
        }


        /// <summary>
        /// Gets the Integer value from the given item for the named column
        /// </summary>
        /// <typeparam name="T">TypeOf object to work with</typeparam>
        /// <param name="item">The item of type T</param>
        /// <param name="columnName">The name of the Int64 column</param>
        /// <returns>Integer value of the named column from the item</returns>
        public static Int64 TryGetColumnValue<T>(this T item, string columnName)
        {
            try
            {
                System.Reflection.PropertyInfo pi = typeof(T).GetProperty(columnName);
                if (pi == null || !pi.PropertyType.Name.Contains("Int64"))
                    return -1;
                return Convert.ToInt64(pi.GetValue(item));
            }
            catch (Exception e) { }
            return -1;
        }
    }
}