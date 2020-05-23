using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HackedDesign.Level
{
    public class FileLevelGenerator : LevelGenerator
    {
        public override Level GenerateLevel(LevelGenTemplate template, int length, int height, int width, int difficulty, int enemies, int traps)
        {
            if (template is null)
            {
                Logger.LogError("FileLevelGenerator", "No level template set");
                return null;
            }

            Logger.Log("FileLevelGenerator", "Using template - ", template.name);

            template.levelLength = length > 0 ? length : template.levelLength;
            template.levelHeight = height > 0 ? height : template.levelHeight;
            template.levelWidth = width > 0 ? width : template.levelWidth;
            template.enemyCount = enemies > 0 ? enemies : template.enemyCount;
            template.trapCount = traps > 0 ? traps : template.trapCount;

            Logger.Log("FileLevelGenerator", "Generating Level ", template.levelLength.ToString(), " x ", template.levelWidth.ToString(), " x ", template.levelHeight.ToString());

            var level = LoadLevelFromFile(template);
            GenerateElements(level);

            return level;
        }

        protected Level LoadLevelFromFile(LevelGenTemplate genTemplate)
        {
            Level level = new Level(genTemplate);
            Logger.Log("FileLevelGenerator", "Loading level from file: Levels/" + genTemplate.levelResource + ".json");
            var jsonTextFile = Resources.Load<TextAsset>("Levels/" + genTemplate.levelResource);
            if (jsonTextFile == null)
            {
                Logger.LogError("FileLevelGenerator", "File not loaded");
                return null;
            }

            JsonUtility.FromJsonOverwrite(jsonTextFile.text, level);

            return level;
        }
    }
}