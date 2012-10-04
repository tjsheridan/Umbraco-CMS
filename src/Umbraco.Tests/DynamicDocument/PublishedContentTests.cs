using System;
using System.Linq;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Tests.TestHelpers;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace Umbraco.Tests.DynamicDocument
{
	/// <summary>
	/// Tests the typed extension methods on IPublishedContent the same way we test the dynamic ones
	/// </summary>
	[TestFixture]
	public class PublishedContentTests : BaseWebTest
	{
		protected override bool RequiresDbSetup
		{
			get { return false; }
		}

		protected override string GetXmlContent(int templateId)
		{
			return @"<?xml version=""1.0"" encoding=""utf-8""?>
<!DOCTYPE root[ 
<!ELEMENT Home ANY>
<!ATTLIST Home id ID #REQUIRED>
<!ELEMENT CustomDocument ANY>
<!ATTLIST CustomDocument id ID #REQUIRED>
]>
<root id=""-1"">
	<Home id=""1046"" parentID=""-1"" level=""1"" writerID=""0"" creatorID=""0"" nodeType=""1044"" template=""" + templateId + @""" sortOrder=""1"" createDate=""2012-06-12T14:13:17"" updateDate=""2012-07-20T18:50:43"" nodeName=""Home"" urlName=""home"" writerName=""admin"" creatorName=""admin"" path=""-1,1046"" isDoc="""">
		<content><![CDATA[]]></content>
		<umbracoUrlAlias><![CDATA[this/is/my/alias, anotheralias]]></umbracoUrlAlias>
		<umbracoNaviHide>1</umbracoNaviHide>
		<Home id=""1173"" parentID=""1046"" level=""2"" writerID=""0"" creatorID=""0"" nodeType=""1044"" template=""" + templateId + @""" sortOrder=""2"" createDate=""2012-07-20T18:06:45"" updateDate=""2012-07-20T19:07:31"" nodeName=""Sub1"" urlName=""sub1"" writerName=""admin"" creatorName=""admin"" path=""-1,1046,1173"" isDoc="""">
			<content><![CDATA[<div>This is some content</div>]]></content>
			<umbracoUrlAlias><![CDATA[page2/alias, 2ndpagealias]]></umbracoUrlAlias>			
			<Home id=""1174"" parentID=""1173"" level=""3"" writerID=""0"" creatorID=""0"" nodeType=""1044"" template=""" + templateId + @""" sortOrder=""2"" createDate=""2012-07-20T18:07:54"" updateDate=""2012-07-20T19:10:27"" nodeName=""Sub2"" urlName=""sub2"" writerName=""admin"" creatorName=""admin"" path=""-1,1046,1173,1174"" isDoc="""">
				<content><![CDATA[]]></content>
				<umbracoUrlAlias><![CDATA[only/one/alias]]></umbracoUrlAlias>
				<creatorName><![CDATA[Custom data with same property name as the member name]]></creatorName>
			</Home>
			<Home id=""1176"" parentID=""1173"" level=""3"" writerID=""0"" creatorID=""0"" nodeType=""1044"" template=""" + templateId + @""" sortOrder=""3"" createDate=""2012-07-20T18:08:08"" updateDate=""2012-07-20T19:10:52"" nodeName=""Sub 3"" urlName=""sub-3"" writerName=""admin"" creatorName=""admin"" path=""-1,1046,1173,1176"" isDoc="""">
				<content><![CDATA[]]></content>
			</Home>
			<CustomDocument id=""1177"" parentID=""1173"" level=""3"" writerID=""0"" creatorID=""0"" nodeType=""1234"" template=""" + templateId + @""" sortOrder=""4"" createDate=""2012-07-16T15:26:59"" updateDate=""2012-07-18T14:23:35"" nodeName=""custom sub 1"" urlName=""custom-sub-1"" writerName=""admin"" creatorName=""admin"" path=""-1,1046,1173,1177"" isDoc="""" />
			<CustomDocument id=""1178"" parentID=""1173"" level=""3"" writerID=""0"" creatorID=""0"" nodeType=""1234"" template=""" + templateId + @""" sortOrder=""4"" createDate=""2012-07-16T15:26:59"" updateDate=""2012-07-16T14:23:35"" nodeName=""custom sub 2"" urlName=""custom-sub-2"" writerName=""admin"" creatorName=""admin"" path=""-1,1046,1173,1178"" isDoc="""" />
		</Home>
		<Home id=""1175"" parentID=""1046"" level=""2"" writerID=""0"" creatorID=""0"" nodeType=""1044"" template=""" + templateId + @""" sortOrder=""3"" createDate=""2012-07-20T18:08:01"" updateDate=""2012-07-20T18:49:32"" nodeName=""Sub 2"" urlName=""sub-2"" writerName=""admin"" creatorName=""admin"" path=""-1,1046,1175"" isDoc=""""><content><![CDATA[]]></content>
		</Home>
		<CustomDocument id=""4444"" parentID=""1046"" level=""2"" writerID=""0"" creatorID=""0"" nodeType=""1234"" template=""" + templateId + @""" sortOrder=""2"" createDate=""2012-07-16T15:26:59"" updateDate=""2012-07-18T14:23:35"" nodeName=""Test"" urlName=""test-page"" writerName=""admin"" creatorName=""admin"" path=""-1,1046,4444"" isDoc="""" />
	</Home>
	<CustomDocument id=""1172"" parentID=""-1"" level=""1"" writerID=""0"" creatorID=""0"" nodeType=""1234"" template=""" + templateId + @""" sortOrder=""2"" createDate=""2012-07-16T15:26:59"" updateDate=""2012-07-18T14:23:35"" nodeName=""Test"" urlName=""test-page"" writerName=""admin"" creatorName=""admin"" path=""-1,1172"" isDoc="""" />
</root>";
		}

		public override void Initialize()
		{
			base.Initialize();

			PropertyEditorValueConvertersResolver.Current = new PropertyEditorValueConvertersResolver(
				new[]
					{
						typeof(DatePickerPropertyEditorValueConverter),
						typeof(TinyMcePropertyEditorValueConverter),
						typeof(YesNoPropertyEditorValueConverter)
					});

			//need to specify a custom callback for unit tests
			DynamicPublishedContent.GetDataTypeCallback = (docTypeAlias, propertyAlias) =>
				{
					if (propertyAlias == "content")
					{
						//return the rte type id
						return Guid.Parse("5e9b75ae-face-41c8-b47e-5f4b0fd82f83");
					}
					return Guid.Empty;
				};
		}

		public override void TearDown()
		{
			base.TearDown();

			PropertyEditorValueConvertersResolver.Reset();
		}

		internal IPublishedContent GetNode(int id)
		{
			var ctx = GetUmbracoContext("/test", 1234);
			var contentStore = new DefaultPublishedContentStore();
			var doc = contentStore.GetDocumentById(ctx, id);
			Assert.IsNotNull(doc);
			return doc;
		}

		[Test]
		public void Children_GroupBy_DocumentTypeAlias()
		{
			var doc = GetNode(1046);

			var found1 = doc.Children.GroupBy("DocumentTypeAlias");

			Assert.AreEqual(2, found1.Count());
			Assert.AreEqual(2, found1.Single(x => x.Key.ToString() == "Home").Count());
			Assert.AreEqual(1, found1.Single(x => x.Key.ToString() == "CustomDocument").Count());
		}

		[Test]
		public void Children_Where_DocumentTypeAlias()
		{
			var doc = GetNode(1046);

			var found1 = doc.Children.Where("DocumentTypeAlias == \"CustomDocument\"");
			var found2 = doc.Children.Where("DocumentTypeAlias == \"Home\"");

			Assert.AreEqual(1, found1.Count());
			Assert.AreEqual(2, found2.Count());
		}

		[Test]
		public void Children_Order_By_Update_Date()
		{
			var doc = GetNode(1173);

			var ordered = doc.Children.OrderBy("UpdateDate");

			var correctOrder = new[] { 1178, 1177, 1174, 1176 };
			for (var i = 0; i < correctOrder.Length; i++)
			{
				Assert.AreEqual(correctOrder[i], ordered.ElementAt(i).Id);
			}

		}

		[Test]
		public void HasProperty()
		{
			var doc = GetNode(1173);

			var hasProp = doc.HasProperty("umbracoUrlAlias");

			Assert.AreEqual(true, (bool)hasProp);

		}


		[Test]
		public void HasValue()
		{
			var doc = GetNode(1173);

			var hasValue = doc.HasValue("umbracoUrlAlias");
			var noValue = doc.HasValue("blahblahblah");

			Assert.IsTrue(hasValue);
			Assert.IsFalse(noValue);
		}


		[Test]
		public void Ancestors_Where_Visible()
		{
			var doc = GetNode(1174);

			var whereVisible = doc.Ancestors().Where("Visible");

			Assert.AreEqual(1, whereVisible.Count());

		}

		[Test]
		public void Visible()
		{
			var hidden = GetNode(1046);
			var visible = GetNode(1173);

			Assert.IsFalse(hidden.IsVisible());
			Assert.IsTrue(visible.IsVisible());
		}

		[Test]
		public void Ensure_TinyMCE_Converted_Type_User_Property()
		{
			var doc = GetNode(1173);

			throw new NotImplementedException("We currently don't have an extension method to return the formatted value using IPropertyValueConverter! This currently only works in the dynamic implementation");

			//Assert.IsTrue(TypeHelper.IsTypeAssignableFrom<IHtmlString>(doc.GetPropertyValue().Content.GetType()));
			//Assert.AreEqual("<div>This is some content</div>", doc.Content.ToString());
		}

		[Test]
		public void Ancestor_Or_Self()
		{
			var doc = GetNode(1173);

			var result = doc.AncestorOrSelf();

			Assert.IsNotNull(result);

			Assert.AreEqual((int)1046, (int)result.Id);
		}

		[Test]
		public void Ancestors_Or_Self()
		{
			var doc = GetNode(1174);

			var result = doc.AncestorsOrSelf();

			Assert.IsNotNull(result);

			Assert.AreEqual(3, result.Count());
			Assert.IsTrue(result.Select(x => ((dynamic)x).Id).ContainsAll(new dynamic[] { 1174, 1173, 1046 }));
		}

		[Test]
		public void Ancestors()
		{
			var doc = GetNode(1174);

			var result = doc.Ancestors();

			Assert.IsNotNull(result);

			Assert.AreEqual(2, result.Count());
			Assert.IsTrue(result.Select(x => ((dynamic)x).Id).ContainsAll(new dynamic[] { 1173, 1046 }));
		}

		[Test]
		public void Descendants_Or_Self()
		{
			var doc = GetNode(1046);

			var result = doc.DescendantsOrSelf();

			Assert.IsNotNull(result);

			Assert.AreEqual(7, result.Count());
			Assert.IsTrue(result.Select(x => ((dynamic)x).Id).ContainsAll(new dynamic[] { 1046, 1173, 1174, 1176, 1175 }));
		}

		[Test]
		public void Descendants()
		{
			var doc = GetNode(1046);

			var result = doc.Descendants();

			Assert.IsNotNull(result);

			Assert.AreEqual(6, result.Count());
			Assert.IsTrue(result.Select(x => ((dynamic)x).Id).ContainsAll(new dynamic[] { 1173, 1174, 1176, 1175 }));
		}

		[Test]
		public void Up()
		{
			var doc = GetNode(1173);

			var result = doc.Up();

			Assert.IsNotNull(result);

			Assert.AreEqual((int)1046, (int)result.Id);
		}

		[Test]
		public void Down()
		{
			var doc = GetNode(1173);

			var result = doc.Down();

			Assert.IsNotNull(result);

			Assert.AreEqual((int)1174, (int)result.Id);
		}

		[Test]
		public void Next()
		{
			var doc = GetNode(1173);

			var result = doc.Next();

			Assert.IsNotNull(result);

			Assert.AreEqual((int)1175, (int)result.Id);
		}

		[Test]
		public void Next_Without_Sibling()
		{
			var doc = GetNode(1178);

			Assert.IsNull(doc.Next());
		}

		[Test]
		public void Previous_Without_Sibling()
		{
			var doc = GetNode(1173);

			Assert.IsNull(doc.Previous());
		}

		[Test]
		public void Previous()
		{
			var doc = GetNode(1176);

			var result = doc.Previous();

			Assert.IsNotNull(result);

			Assert.AreEqual((int)1174, (int)result.Id);
		}
	}
}