  USE [Master]
  GO

  IF db_id('AlStudente') IS NULL
    CREATE DATABASE [AlStudente]
  GO

  USE [AlStudente]
  GO
  DROP TABLE IF EXISTS [UserProfile];
  DROP TABLE IF EXISTS [UserType];
  DROP TABLE IF EXISTS [Teacher];
  DROP TABLE IF EXISTS [Student];
  DROP TABLE IF EXISTS [Instrument];
  DROP TABLE IF EXISTS [TeacherNote];
  DROP TABLE IF EXISTS [LessonNote];
  DROP TABLE IF EXISTS [LessonComment];
  DROP TABLE IF EXISTS [LessonDay];
  DROP TABLE IF EXISTS [LessonTime];
  DROP TABLE IF EXISTS [Level];

  CREATE TABLE [UserType] (
    [Id] int PRIMARY KEY IDENTITY,
    [Name] nvarchar(255)
  )
  GO

  CREATE TABLE [Instrument] (
    [Id] int PRIMARY KEY IDENTITY,
    [Name] nvarchar(255) NOT NULL
  )
  GO

  CREATE TABLE [LessonDay] (
    [Id] int PRIMARY KEY IDENTITY,
    [Day] nvarchar(255)
  )
  GO

  CREATE TABLE [LessonTime] (
    [Id] int PRIMARY KEY IDENTITY,
    [Time] nvarchar(255),
    [Length] int
  )
  GO

  CREATE TABLE [Level] (
    [Id] int PRIMARY KEY IDENTITY,
    [Name] nvarchar(255),
  )
  GO

  CREATE TABLE [UserProfile] (
    [Id] int PRIMARY KEY IDENTITY,
    [FirebaseUserId] nvarchar(255) NOT NULL,
    [FirstName] nvarchar(255),
    [LastName] nvarchar(255),
    [DisplayName] nvarchar(255) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [CreateDateTime] datetime NOT NULL,
    [ImageLocation] nvarchar(255),
    [UserTypeId] int NOT NULL,
    [InstrumentId] int,
    [Bio] nvarchar(255),

    CONSTRAINT [FK_User_UserType] FOREIGN KEY ([UserTypeId]) REFERENCES [UserType] ([Id]),
    CONSTRAINT [FK_User_Instrument] FOREIGN KEY ([InstrumentId]) REFERENCES [Instrument] ([Id])
  )
  GO

  CREATE TABLE [Teacher] (
    [Id] int PRIMARY KEY IDENTITY,
    [UserId] int NOT NULL,
    [AcceptingStudents] bit NOT NULL,
    [LessonRate] int,

    CONSTRAINT [FK_Teacher_UserProfile] FOREIGN KEY ([UserId]) REFERENCES [UserProfile] ([Id])
  )
  GO

  CREATE TABLE [Student] (
    [Id] int PRIMARY KEY IDENTITY,
    [UserId] int NOT NULL,
    [TeacherId] int,
    [DOB] date,
    [StartDate] date,
    [PlayingSince] int,
    [LevelId] int,
    [LessonDayId] int,
    [LessonTimeId] int,

    CONSTRAINT [FK_Student_Teacher] FOREIGN KEY ([TeacherId]) REFERENCES [Teacher] ([Id]),
    CONSTRAINT [FK_Student_Level] FOREIGN KEY ([LevelId]) REFERENCES [Level] ([Id]),
    CONSTRAINT [FK_Student_LessonDay] FOREIGN KEY ([LessonDayId]) REFERENCES [LessonDay] ([Id]),
    CONSTRAINT [FK_Student_LessonTime] FOREIGN KEY ([LessonTimeId]) REFERENCES [LessonTime] ([Id])
  )
  GO

  CREATE TABLE [TeacherNote] (
    [Id] int PRIMARY KEY IDENTITY,
    [TeacherId] int NOT NULL,
    [StudentId] int NOT NULL,
    [Title] nvarchar(255),
    [Content] nvarchar(255),
    [CreateDateTime] datetime NOT NULL,

    CONSTRAINT [FK_TeacherNote_Teacher] FOREIGN KEY ([TeacherId]) REFERENCES [Teacher] ([Id]),
    CONSTRAINT [FK_TeacherNote_Student] FOREIGN KEY ([StudentId]) REFERENCES [Student] ([Id])
  )
  GO

  CREATE TABLE [LessonNote] (
    [Id] int PRIMARY KEY IDENTITY,
    [TeacherId] int NOT NULL,
    [StudentId] int NOT NULL,
    [LessonNumber] int,
    [Title] nvarchar(255),
    [Content] nvarchar(255),
    [CreateDateTime] datetime NOT NULL,
    [LessonDate] date,

    CONSTRAINT [FK_LessonNote_Teacher] FOREIGN KEY ([TeacherId]) REFERENCES [Teacher] ([Id]),
    CONSTRAINT [FK_LessonNote_Student] FOREIGN KEY ([StudentId]) REFERENCES [Student] ([Id])
  )
  GO

  CREATE TABLE [LessonComment] (
    [Id] int PRIMARY KEY IDENTITY,
    [LessonId] int NOT NULL,
    [UserId] int NOT NULL,
    [Content] nvarchar(255),
    [CreateDateTime] datetime NOT NULL,

    CONSTRAINT [FK_LessonComment_Lesson] FOREIGN KEY ([LessonId]) REFERENCES [LessonNote] ([Id]),
    CONSTRAINT [FK_LessonComment_User] FOREIGN KEY ([UserId]) REFERENCES [UserProfile] ([Id])
  )
  GO