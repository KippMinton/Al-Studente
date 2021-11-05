USE [AlStudente];
GO

set identity_insert [UserType] on
insert into [UserType] ([ID], [Name]) VALUES (1, 'Admin'), (2, 'Teacher'), (3, 'Student');
set identity_insert [UserType] off

set identity_insert [Instrument] on
insert into [Instrument] ([Id], [Name]) 
values (0, 'no instrument selected'), (1, 'Piano'), (2, 'Voice'), (3, 'Guitar'), (4, 'Rock Guitar'), (5, 'Classical Guitar'), (6, 'Jazz Guitar'), (7, 'Bass Guitar'), (8, 'Violin'), (9, 'Viola'), (10, 'Cello'), (11, 'Double Bass'), (12, 'Drums');
set identity_insert [Instrument] off

set identity_insert [LessonDay] on
insert into [LessonDay] ([Id], [Day])
values (1, 'Sunday'), (2, 'Monday'), (3, 'Tuesday'), (4, 'Wednesday'), (5, 'Thursday'), (6, 'Friday'), (7, 'Saturday');
set identity_insert [LessonDay] off

set identity_insert [LessonTime] on
insert into [LessonTime] ([Id], [Time])
values (1, '10:00 am'), (2, '10:30 am'), (3, '11:00 am'), (4, '11:30 am'), (5, '12:00 pm'), (6, '12:30 pm'), (7, '1:00 pm'), (8, '1:30 pm'), (9, '2:00 pm'), (10, '2:30pm'), (11, '3:00 pm'), (12, '3:30 pm'), (13, '4:00 pm'), (14, '4:30 pm'), (15, '5:00 pm'), (16, '5:30 pm'), (17, '6:00 pm'), (18, '6:30 pm'), (19, '7:00 pm'), (20, '7:30 pm'), (21, '8:00 pm'), (22, '8:30 pm'), (23, '9:00 pm'), (24, '9:30 pm');
set identity_insert [LessonTime] off

set identity_insert [Level] on
insert into [Level] ([Id], [Name])
values (1, 'Beginner'), (2, 'Intermediate'), (3, 'Advanced');
set identity_insert [Level] off

-- empty teacher user for soft deleting students from teachers' rosters --
set identity_insert [Teacher] on
INSERT INTO
Teacher (Id, UserId, AcceptingStudents, LessonRate)
VALUES(0, 0, 0, 0)
set identity_insert [Teacher] off
set identity_insert [UserProfile] on
INSERT INTO
[UserProfile] ([Id], FirebaseUserId, DisplayName, Email, CreateDateTime, UserTypeId)
VALUES(0, 'NoTeacher', 'NoTeacher', 'NoTeacher', GETDATE(), 2);
set identity_insert [UserProfile] off
