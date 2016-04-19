using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Template
{
	public class TemplateClass : MonoBehaviour 
	{
        public string LessonID { get; set; }
		public string title { get; set; }
		public string previewData{ get; set; }
		public string learnerBG{ get; set; }
		public string module{ get; set; }
		public string learnerLevel{ get; set; }
		public string duration{ get; set; }
		public ArrayList objective{ get; set; }
		public ArrayList textData{ get; set; }
		public ArrayList imageData{ get; set; }
		public string movieData{ get; set; }
		public int pageCount{ get; set; }
		public int pageID{ get; set; }
		public int templateID{ get; set; }
		public int subjectNumber{ get; set; }
		public string subjectType{ get; set; }
		public string subject{ get; set; }
		public ArrayList answer{ get; set; }
		public string trueAnswer{ get; set; }
		public string reviewPage{ get; set; }


		UnityEngine.TextAsset txt;
		JsonData learningInfo;

		public TemplateClass (string jsonData,int pageID) 
		{
			initList ();
			loadData (jsonData,pageID);

			switch (this.templateID)
			{
			case 1:getDataFromPreview ();break;
			case 2:getDataFromBasicTheory ();break;
			case 3:getDataFromLearning(pageID);break;
			case 4:getDataFromConclusion();break;
			case 5:getDataFromTesting(1);break;
			default:break;
			}
		}

		public void initList()
		{
			objective = new ArrayList ();
			textData = new ArrayList ();
			imageData = new ArrayList ();
			answer = new ArrayList ();
		}

		public void loadData(string jsonData,int PageID)
		{ 
			learningInfo = JsonMapper.ToObject (jsonData);
#if UNITY_EDITOR
			txt = Resources.Load("SimulateJsonV2") as TextAsset; 
			learningInfo = JsonMapper.ToObject (txt.text);
#endif

			this.pageCount = int.Parse( learningInfo ["PageTatalCount"].ToString() );
			this.pageID = PageID;
			this.templateID = int.Parse( learningInfo ["Page"] [this.pageID.ToString ()] ["TemplateID"].ToString() );
			this.title = learningInfo ["Page"] [this.pageID.ToString ()] ["Title"].ToString();
            this.LessonID = learningInfo["LessonID"].ToString();
		}

		public void getDataFromPreview()
		{
			this.previewData = learningInfo ["Page"] [this.pageID.ToString ()] ["PreviewData"].ToString ();
			this.learnerBG = learningInfo ["Page"] [this.pageID.ToString ()] ["TextData"] ["LearnerBG"].ToString ();
			this.module = learningInfo ["Page"] [this.pageID.ToString ()] ["Module"].ToString ();
			this.learnerLevel = learningInfo ["Page"] [this.pageID.ToString ()] ["LearnerLevel"].ToString ();
			this.duration = learningInfo ["Page"] [this.pageID.ToString ()] ["Duration"].ToString ();
			for(int i = 0; i < learningInfo ["Page"] [this.pageID.ToString ()] ["TextData"] ["Objective"].Count;i++)
			{
				this.objective.Add( learningInfo ["Page"] [this.pageID.ToString ()] ["TextData"] ["Objective"][(i+1).ToString()].ToString() );
			}
		}

		public void getDataFromBasicTheory()
		{
			for(int i = 0; i < learningInfo ["Page"] [this.pageID.ToString ()] ["ImageData"] .Count;i++)
			{
				this.imageData.Add( learningInfo ["Page"] [this.pageID.ToString ()] ["ImageData"] [(i+1).ToString()].ToString() );
			}
			for(int j = 0; j < learningInfo ["Page"] [this.pageID.ToString ()] ["TextData"] .Count;j++)
			{
				this.textData.Add( learningInfo ["Page"] [this.pageID.ToString ()] ["TextData"] [(j+1).ToString()].ToString() );
			}
		}

		public void getDataFromLearning(int pageID)
		{
			this.movieData = learningInfo ["Page"] [this.pageID.ToString ()] ["MovieData"].ToString();
		}

		public void getDataFromConclusion()
		{
			for(int j = 0; j < learningInfo ["Page"] [this.pageID.ToString ()] ["TextData"] .Count;j++)
			{
				this.textData.Add( learningInfo ["Page"] [this.pageID.ToString ()] ["TextData"] [(j+1).ToString()].ToString() );
			}
		}

		public void getDataFromTesting(int id)
		{
			this.answer.Clear();
			this.subjectNumber = int.Parse( learningInfo ["Page"] [this.pageID.ToString ()]  ["SubjectNumber"].ToString());
			this.subjectType = learningInfo ["Page"] [this.pageID.ToString ()][id.ToString()] ["SubjectType"].ToString();
			this.subject =  learningInfo ["Page"] [this.pageID.ToString ()][id.ToString()]  ["Subject"].ToString();

			for(int i = 0; i < learningInfo ["Page"] [this.pageID.ToString ()] [id.ToString()] ["Answer"] .Count;i++)
			{
				this.answer.Add( learningInfo ["Page"] [this.pageID.ToString ()] [id.ToString()] ["Answer"] [(i+1).ToString()].ToString() );
			}
			this.trueAnswer = learningInfo ["Page"] [this.pageID.ToString ()] [id.ToString()] ["TrueAnswer"].ToString ();
			this.reviewPage = learningInfo ["Page"] [this.pageID.ToString ()] [id.ToString()] ["ReviewPage"].ToString();
		}
	}
}