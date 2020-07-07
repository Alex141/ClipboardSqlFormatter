using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClipboardSqlFormatter.Tests
{
    [TestClass]
    public class CommonUnitTests
    {
        [TestMethod]
        public void SqlStartsWithoutEmptySpaceTest()
        {
            string inputSql = "exec sp_executesql N' SELECT TOP 1000 \"obj\".\"Actual\", \"doc\".\"DescrDoc\" AS \"Annotation\", \"doc\".\"Composition\", \"obj\".\"CreateDateTime\", \"obj\".\"EditorID\" AS \"CUREDITOR\", \"obj\".\"EditorID\" AS \"CurrentEditorID\", (select case when (select stateid from lderc where id=doc.id)=6 then (SELECT TOP 1 o.ExecDateTime FROM LDOPERATION o WHERE o.OperTypeID = 2142 AND o.ObjectID = doc.ID ORDER BY o.ID DESC)else null end) AS \"DeleteDate\", (select case when (select stateid from lderc where id=doc.id)=6 then (SELECT TOP 1 v.Name FROM LDOPERATION o JOIN LDVocabulary v ON o.MemberID = v.ID  WHERE o.OperTypeID = 2142 AND o.ObjectID = doc.ID ORDER BY o.ID DESC) else '''' end) AS \"DeletedByUser\", \"abstDoc\".\"EntityName\", \"doc\".\"ID\", \"obj\".\"LastEditDateTime\" AS \"LastEditDate\", \"obj\".\"LastEditDateTime\", \"LASTEDITSESSION.USER.voc\".\"Name\" AS \"LastEditor\", \"abstDoc\".\"Name\", \"doc\".\"N\" AS \"Number\", \"obj\".\"ObjectTypeID\" AS \"ObjectType\", \"obj\".\"ObjID\", \"doc\".\"RegDate\" AS \"RegistrationDate\", \"doc\".\"DocN\" AS \"RegistrationNumber\", \"CHANGE.LDObjectChange\".\"LastChangedDate\" AS \"ServerLastUpdateTime\", \"doc\".\"SheafCount\", \"doc\".\"StateID\" AS \"State\", \"obj\".\"UID\", \"doc\".\"VersionCount\", \"obj\".\"VersionN\", \"doc\".\"Viewers\", charindex(@V0, \"doc\".\"Viewers\") AS \"ViewFlag\", \"doc\".\"N\" AS \"XNumber\", \"doc\".\"ID\" AS \"DatabaseID\", \"doc\".\"AccessID\", \"obj\".\"ObjectTypeID\" AS \"CardID\", \"doc\".\"JournalID\" AS \"ContainerID\", \"obj\".\"EditorID\" AS \"CurEditorID\", \"doc\".\"DeliveryTypeID\", \"doc\".\"StateDocumentID\" AS \"DocumentStateID\", \"doc\".\"DocumTypeID\", \"doc\".\"JournalID\" FROM LDERC \"doc\" INNER JOIN LDObject \"obj\" ON (\"doc\".\"ID\" = \"obj\".\"ID\") INNER JOIN VIEW30ABSTRACTDOC \"abstDoc\" ON (\"doc\".\"ID\" = \"abstDoc\".\"ID\") LEFT OUTER JOIN LDSession \"LASTEDITSESSION.sess\" ON (\"obj\".\"LastEditSessionID\" = \"LASTEDITSESSION.sess\".\"ID\") LEFT OUTER JOIN LDVocabulary \"LASTEDITSESSION.USER.voc\" ON (\"LASTEDITSESSION.sess\".\"UserID\" = \"LASTEDITSESSION.USER.voc\".\"ID\") LEFT OUTER JOIN LDObjectChange \"CHANGE.LDObjectChange\" ON (\"obj\".\"ID\" = \"CHANGE.LDObjectChange\".\"ObjectID\") LEFT OUTER JOIN LDJournal \"ContainerID.jrn\" ON (\"doc\".\"JournalID\" = \"ContainerID.jrn\".\"ID\") WHERE  (  (  (  (  (  ( \"doc\".\"StateID\" <> @V1 )  AND \"doc\".\"JournalID\" = @V2 )  AND  ( \"doc\".\"AccessID\" <= @V3 OR  (  ( \"doc\".\"AccessID\" = 999 OR  ( \"doc\".\"AccessID\" = 1000 AND  ( @V4 = @V5 OR \"doc\".\"ID\" IN  (  SELECT \"LDMemberObject\".\"ObjectID\" AS \"ID\" FROM LDMemberObject \"LDMemberObject\" WHERE \"LDMemberObject\".\"MemberID\" = @V6 )  )  )  )  AND \"doc\".\"ID\" IN  (  SELECT \"LDMemberObject\".\"ObjectID\" AS \"ID\" FROM LDMemberObject \"LDMemberObject\" WHERE \"LDMemberObject\".\"MemberID\" IN  ( select /*10*/ * from dbo.LD30_Str2List (@L7) )  )  )  )  )  )  AND  ( \"doc\".\"StateID\" <> 6 OR \"doc\".\"JournalID\" IN  (  SELECT \"LDRightObject\".\"ObjectID\" AS \"ID\" FROM LDRightObject \"LDRightObject\" WHERE  (  ( \"LDRightObject\".\"RightID\" = 2145 AND \"LDRightObject\".\"MemberID\" IN  ( select /*10*/ * from dbo.LD30_Str2List (@L8) )  )  OR  ( \"LDRightObject\".\"RightID\" IN  (  SELECT \"LDRight\".\"ID\" FROM LDRight \"LDRight\" WHERE  ( \"LDRight\".\"ID\" = 2145 AND \"LDRight\".\"Enabled\" = ''-'' )  )  )  )  )  )  )  AND  ( \"doc\".\"StateID\" <> 5 AND \"ContainerID.jrn\".\"Name\" <> ''Скрытый журнал для проектов'' )  )  ORDER BY \"doc\".\"ID\" DESC',N'@V0 nvarchar(6),@V1 bigint,@V2 bigint,@V3 bigint,@V4 bigint,@V5 bigint,@V6 bigint,@L7 varchar(28),@L8 varchar(28)',@V0=N',1000,',@V1=6,@V2=462022,@V3=1,@V4=1000,@V5=1000,@V6=1000,@L7='1000,1010,1011,356938,381781',@L8='1000,1010,1011,356938,381781'";

            #region long output
            string expectedOutputSql = @"SELECT TOP 1000 ""obj"".""Actual""
	,""doc"".""DescrDoc"" AS ""Annotation""
	,""doc"".""Composition""
	,""obj"".""CreateDateTime""
	,""obj"".""EditorID"" AS ""CUREDITOR""
	,""obj"".""EditorID"" AS ""CurrentEditorID""
	,(
		SELECT CASE 
				WHEN (
						SELECT stateid
						FROM lderc
						WHERE id = doc.id
						) = 6
					THEN (
							SELECT TOP 1 o.ExecDateTime
							FROM LDOPERATION o
							WHERE o.OperTypeID = 2142
								AND o.ObjectID = doc.ID
							ORDER BY o.ID DESC
							)
				ELSE NULL
				END
		) AS ""DeleteDate""
	,(
		SELECT CASE 
				WHEN (
						SELECT stateid
						FROM lderc
						WHERE id = doc.id
						) = 6
					THEN (
							SELECT TOP 1 v.NAME
							FROM LDOPERATION o
							JOIN LDVocabulary v ON o.MemberID = v.ID
							WHERE o.OperTypeID = 2142
								AND o.ObjectID = doc.ID
							ORDER BY o.ID DESC
							)
				ELSE ''
				END
		) AS ""DeletedByUser""
	,""abstDoc"".""EntityName""
	,""doc"".""ID""
	,""obj"".""LastEditDateTime"" AS ""LastEditDate""
	,""obj"".""LastEditDateTime""
	,""LASTEDITSESSION.USER.voc"".""Name"" AS ""LastEditor""
	,""abstDoc"".""Name""
	,""doc"".""N"" AS ""Number""
	,""obj"".""ObjectTypeID"" AS ""ObjectType""
	,""obj"".""ObjID""
	,""doc"".""RegDate"" AS ""RegistrationDate""
	,""doc"".""DocN"" AS ""RegistrationNumber""
	,""CHANGE.LDObjectChange"".""LastChangedDate"" AS ""ServerLastUpdateTime""
	,""doc"".""SheafCount""
	,""doc"".""StateID"" AS ""State""
	,""obj"".""UID""
	,""doc"".""VersionCount""
	,""obj"".""VersionN""
	,""doc"".""Viewers""
	,charindex(N',1000,', ""doc"".""Viewers"") AS ""ViewFlag""
	,""doc"".""N"" AS ""XNumber""
	,""doc"".""ID"" AS ""DatabaseID""
	,""doc"".""AccessID""
	,""obj"".""ObjectTypeID"" AS ""CardID""
	,""doc"".""JournalID"" AS ""ContainerID""
	,""obj"".""EditorID"" AS ""CurEditorID""
	,""doc"".""DeliveryTypeID""
	,""doc"".""StateDocumentID"" AS ""DocumentStateID""
	,""doc"".""DocumTypeID""
	,""doc"".""JournalID""
FROM LDERC ""doc""
INNER JOIN LDObject ""obj"" ON (""doc"".""ID"" = ""obj"".""ID"")
INNER JOIN VIEW30ABSTRACTDOC ""abstDoc"" ON (""doc"".""ID"" = ""abstDoc"".""ID"")
LEFT OUTER JOIN LDSession ""LASTEDITSESSION.sess"" ON (""obj"".""LastEditSessionID"" = ""LASTEDITSESSION.sess"".""ID"")
LEFT OUTER JOIN LDVocabulary ""LASTEDITSESSION.USER.voc"" ON (""LASTEDITSESSION.sess"".""UserID"" = ""LASTEDITSESSION.USER.voc"".""ID"")
LEFT OUTER JOIN LDObjectChange ""CHANGE.LDObjectChange"" ON (""obj"".""ID"" = ""CHANGE.LDObjectChange"".""ObjectID"")
LEFT OUTER JOIN LDJournal ""ContainerID.jrn"" ON (""doc"".""JournalID"" = ""ContainerID.jrn"".""ID"")
WHERE (
		(
			(
				(
					(
						(""doc"".""StateID"" <> 6)
						AND ""doc"".""JournalID"" = 462022
						)
					AND (
						""doc"".""AccessID"" <= 1
						OR (
							(
								""doc"".""AccessID"" = 999
								OR (
									""doc"".""AccessID"" = 1000
									AND (
										1000 = 1000
										OR ""doc"".""ID"" IN (
											SELECT ""LDMemberObject"".""ObjectID"" AS ""ID""
											FROM LDMemberObject ""LDMemberObject""
											WHERE ""LDMemberObject"".""MemberID"" = 1000
											)
										)
									)
								)
							AND ""doc"".""ID"" IN (
								SELECT ""LDMemberObject"".""ObjectID"" AS ""ID""
								FROM LDMemberObject ""LDMemberObject""
								WHERE ""LDMemberObject"".""MemberID"" IN (
										SELECT /*10*/ *
										FROM dbo.LD30_Str2List('1000,1010,1011,356938,381781')
										)
								)
							)
						)
					)
				)
			AND (
				""doc"".""StateID"" <> 6
				OR ""doc"".""JournalID"" IN (
					SELECT ""LDRightObject"".""ObjectID"" AS ""ID""
					FROM LDRightObject ""LDRightObject""
					WHERE (
							(
								""LDRightObject"".""RightID"" = 2145
								AND ""LDRightObject"".""MemberID"" IN (
									SELECT /*10*/ *
									FROM dbo.LD30_Str2List('1000,1010,1011,356938,381781')
									)
								)
							OR (
								""LDRightObject"".""RightID"" IN (
									SELECT ""LDRight"".""ID""
									FROM LDRight ""LDRight""
									WHERE (
											""LDRight"".""ID"" = 2145
											AND ""LDRight"".""Enabled"" = '-'
											)
									)
								)
							)
					)
				)
			)
		AND (
			""doc"".""StateID"" <> 5
			AND ""ContainerID.jrn"".""Name"" <> 'Скрытый журнал для проектов'
			)
		)
ORDER BY ""doc"".""ID"" DESC
";
#endregion

            var formatter = new SqlFormatter();

            string outputSql;
            Assert.IsTrue(formatter.FormatSql(inputSql, out outputSql));
            Assert.AreEqual(expectedOutputSql, outputSql);
        }

        [TestMethod]
        public void InputSqlStartsWithEmptySpaceTest()
        {
            string inputSql = "  exec sp_executesql N' SELECT \"LDRightObject\".\"RightID\", \"LDRightObject\".\"FilterID\", \"LDRightObject\".\"MemberID\", \"LDRightObject\".\"ObjectID\" AS \"ID\" FROM LDRightObject \"LDRightObject\" WHERE  (  ( \"LDRightObject\".\"MemberID\" = @V0 OR \"LDRightObject\".\"MemberID\" = @V1 OR \"LDRightObject\".\"MemberID\" = @V2 OR \"LDRightObject\".\"MemberID\" = @V3 OR \"LDRightObject\".\"MemberID\" = @V4 )  AND \"LDRightObject\".\"ObjectID\" = @V5 AND \"LDRightObject\".\"RightID\" = @V6 ) ',N'@V0 bigint,@V1 bigint,@V2 bigint,@V3 bigint,@V4 bigint,@V5 bigint,@V6 bigint',@V0=1010,@V1=1011,@V2=356938,@V3=381781,@V4=1000,@V5=462022,@V6=513";
            
            string expectedOutputSql = @"SELECT ""LDRightObject"".""RightID""
	,""LDRightObject"".""FilterID""
	,""LDRightObject"".""MemberID""
	,""LDRightObject"".""ObjectID"" AS ""ID""
FROM LDRightObject ""LDRightObject""
WHERE (
		(
			""LDRightObject"".""MemberID"" = 1010
			OR ""LDRightObject"".""MemberID"" = 1011
			OR ""LDRightObject"".""MemberID"" = 356938
			OR ""LDRightObject"".""MemberID"" = 381781
			OR ""LDRightObject"".""MemberID"" = 1000
			)
		AND ""LDRightObject"".""ObjectID"" = 462022
		AND ""LDRightObject"".""RightID"" = 513
		)
";

            var formatter = new SqlFormatter();

            string outputSql;
            Assert.IsTrue(formatter.FormatSql(inputSql, out outputSql));
            Assert.AreEqual(expectedOutputSql, outputSql);
        }

        [TestMethod]
        public void CanHandleMoreThan10Parameters()
        {
            string inputSql = @"EXEC sp_executesql N'select @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15'
	            ,N'@p0 int,@p1 int,@p2 int,@p3 nvarchar(255),@p4 bit,@p5 varchar(8000),@p6 nvarchar(255),@p7 datetime2(7),@p8 datetime2(7),@p9 nvarchar(255),@p10 nvarchar(255),@p11 nvarchar(255),@p12 nvarchar(255),@p13 nvarchar(255),@p14 datetime2(7),@p15 datetime2(7)'
	            ,@p0 = 1
	            ,@p1 = 1
	            ,@p2 = 1
	            ,@p3 = N'number 3'
	            ,@p4 = 1
	            ,@p5 = 'number 5'
	            ,@p6 = N'number 6'
	            ,@p7 = '2020-01-01 00:00:00'
	            ,@p8 = '2020-07-01 12:56:22'
	            ,@p9 = N'number 9'
	            ,@p10 = N'number 10'
	            ,@p11 = N'number 11'
	            ,@p12 = N'number 12'
	            ,@p13 = N'number 13'
	            ,@p14 = '2020-01-01 00:00:00'
	            ,@p15 = '2020-07-01 12:56:22'";

            string expectedOutputSql = @"SELECT 1
	,1
	,1
	,N'number 3'
	,1
	,'number 5'
	,N'number 6'
	,'2020-01-01 00:00:00'
	,'2020-07-01 12:56:22'
	,N'number 9'
	,N'number 10'
	,N'number 11'
	,N'number 12'
	,N'number 13'
	,'2020-01-01 00:00:00'
	,'2020-07-01 12:56:22'
";

            var formatter = new SqlFormatter();

            Assert.IsTrue(formatter.FormatSql(inputSql, out var outputSql));
            Assert.AreEqual(expectedOutputSql, outputSql);
        }
    }
}
