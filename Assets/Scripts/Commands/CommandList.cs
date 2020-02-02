using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu (menuName = "Command/List")]
public class CommandList : Command
{
   private UIManager UI;
   private FileSystem filesystem;
   
   public override void InitializeCommand()
   {
      UI = GameObject.FindObjectOfType<UIManager>();

      filesystem = GameObject.FindObjectOfType<FileSystem>();
   }

   public override void ExecuteCommand()
   {
      FileData[] files = filesystem.ReturnFilesInFolder();
      
      string testString = String.Empty;;

      for (int i = 0; i < files.Length; i++)
      {
         testString += files[i].name;
         testString += " - ";
         testString += files[i].lastAccessDate;
         testString += " - ";
         testString += files[i].sizeBytes;
         testString += "\n";
      }
      
      UI.SetNewBacklogText(testString);
   }
}
