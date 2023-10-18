INSERT INTO [dbo].[Platform]
           ([uniqueName]
           ,[latitude]
           ,[longitude]
           ,[createdAt]
           ,[updatedAt])
     VALUES
     ( 'Platform1','1.012','0.1231', convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:24.123')),     
     ( 'Platform2','1.018','0.1284', convert(datetime,'2010-01-15T01:14:57.543'),convert(datetime,'2010-01-15T01:14:57.543')),
     ( 'Platform3','1.024','0.1337', convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:24.124')),
     ( 'Platform4','1.112','0.0231', convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:24.125')),
     ( 'Platform5','1.009','0.1333', convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:25.123')),
     ( 'Platform6','1.017','0.1261', convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:26.123')),
     ( 'Platform7','1.012','0.1231', convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:27.123')),
     ( 'Platform8','1.012','0.1231', convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:28.123')),
     ( 'Platform9','1.012','0.1231', convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:29.123')),
     ( 'Platform10','1.012','0.1231',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:20.123'))


     INSERT INTO [dbo].[Well]
           ([platformId]
           ,[uniqueName]
           ,[latitude]
           ,[longitude]
           ,[createdAt]
           ,[updatedAt])
     VALUES
     (1,'Well11','37.06257','18.406885',convert(datetime,'2023-10-18T12:01:49.531'),convert(datetime,'2023-10-18T12:01:49.531')),
     (1,'Well12','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:25.123')),
     (1,'Well13','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:26.123')),
     (2,'Well14','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:27.123')),
     (2,'Well15','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:28.123')),
     (2,'Well16','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:29.123')),
     (3,'Well17','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:22.123')),
     (3,'Well18','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:34.123')),
     (4,'Well19','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:44.123')),
     (4,'Well20','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:54.123')),
     (5,'Well21','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:11:34.123')),
     (5,'Well22','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:12:24.123')),
     (6,'Well23','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:11:24.123')),
     (6,'Well24','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:11:24.123')),
     (7,'Well25','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:12:24.123')),
     (7,'Well26','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:09:24.123')),
     (8,'Well27','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:01:24.123')),
     (8,'Well28','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:07:24.123')),
     (9,'Well29','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:06:24.123')),
     (9,'Well30','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:04:24.123')),
     (9,'Well31','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:03:24.123')),
     (9,'Well32','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:24.123')),
     (9,'Well33','37.06257','18.406885',convert(datetime,'2010-01-12T01:10:24.123'),convert(datetime,'2010-01-12T01:10:24.123'))
