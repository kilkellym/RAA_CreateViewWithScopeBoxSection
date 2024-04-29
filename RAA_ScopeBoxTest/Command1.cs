namespace RAA_ScopeBoxTest
{
	[Transaction(TransactionMode.Manual)]
	public class Command1 : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			// this is a variable for the Revit application
			UIApplication uiapp = commandData.Application;

			// this is a variable for the current Revit model
			Document doc = uiapp.ActiveUIDocument.Document;

			using (Transaction t = new Transaction(doc))
			{
				t.Start("Create 3D View with Scope Box");

				// create 3D view
				View3D newView = View3D.CreateIsometric(doc, Get3DViewFamilyTypeId(doc));
				newView.Name = "3D View 1";

				// get scope box
				Element scopeBox = GetScopeBoxByName(doc, "Scope Box 1");

				// apply scope box to view as section view
				newView.SetSectionBox(scopeBox.get_BoundingBox(doc.ActiveView));

				t.Commit();
			}	

			return Result.Succeeded;
		}

		public static List<ViewFamilyType> GetAllViewTypes(Document m_doc)
		{
			//get list of view types
			FilteredElementCollector m_colVT = new FilteredElementCollector(m_doc);
			m_colVT.OfClass(typeof(ViewFamilyType));

			List<ViewFamilyType> m_vt = new List<ViewFamilyType>();
			foreach (ViewFamilyType x in m_colVT.ToElements())
			{
				m_vt.Add(x);
			}

			return m_vt;
		}

		public static ElementId Get3DViewFamilyTypeId(Document curDoc)
		{
			//get list of view types
			List<ViewFamilyType> vTypes = GetAllViewTypes(curDoc);

			foreach (ViewFamilyType x in vTypes)
			{
				if (x.ViewFamily == ViewFamily.ThreeDimensional)
				{
					return x.Id;
				}
			}

			return null;
		}
		public static Element GetScopeBoxByName(Document curDoc, string sbName)
		{
			// get all scope boxes
			List<Element> sbList = GetAllScopeBoxes(curDoc);

			// loop through list and look for match
			foreach (Element curElem in sbList)
			{
				if (curElem.Name == sbName)
				{
					return curElem;
				}
			}

			return null;
		}
		public static List<Element> GetAllScopeBoxes(Document curDoc)
		{
			// get all scope boxes
			FilteredElementCollector f = new FilteredElementCollector(curDoc);
			f.OfCategory(BuiltInCategory.OST_VolumeOfInterest);

			return f.ToList();
		}
		internal static PushButtonData GetButtonData()
		{
			// use this method to define the properties for this command in the Revit ribbon
			string buttonInternalName = "btnCommand1";
			string buttonTitle = "Button 1";

			Utils.ButtonDataClass myButtonData1 = new Utils.ButtonDataClass(
				buttonInternalName,
				buttonTitle,
				MethodBase.GetCurrentMethod().DeclaringType?.FullName,
				Properties.Resources.Blue_32,
				Properties.Resources.Blue_16,
				"This is a tooltip for Button 1");

			return myButtonData1.Data;
		}
	}

}
