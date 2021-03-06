USE [Zavrsni]
GO
/****** Object:  Table [dbo].[BelongsToGroup]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BelongsToGroup](
	[IDgroup] [int] NOT NULL,
	[IDuser] [int] NOT NULL,
	[TimeChanged] [datetime] NULL,
 CONSTRAINT [PK_PripadaGrupi] PRIMARY KEY CLUSTERED 
(
	[IDgroup] ASC,
	[IDuser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[City]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[IDcity] [int] IDENTITY(2,1) NOT NULL,
	[IDcountry] [int] NOT NULL,
	[CityName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Grad] PRIMARY KEY CLUSTERED 
(
	[IDcity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_City] UNIQUE NONCLUSTERED 
(
	[CityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Content]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Content](
	[IDcontent] [int] IDENTITY(1,1) NOT NULL,
	[IDcontentType] [int] NOT NULL,
	[IDauthor] [int] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[URL] [nvarchar](100) NULL,
	[IDeditor] [int] NULL,
	[TimeChanged] [datetime] NULL,
	[Title] [nvarchar](100) NULL,
	[IsCopied] [bit] NULL,
	[DataRow] [int] NULL,
	[DataCol] [int] NULL,
	[DataSizeX] [int] NULL,
	[DataSizeY] [int] NULL,
 CONSTRAINT [PK_Sadrzaj] PRIMARY KEY CLUSTERED 
(
	[IDcontent] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContentComment]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentComment](
	[IDcomment] [int] IDENTITY(1,1) NOT NULL,
	[IDcontent] [int] NOT NULL,
	[IDuser] [int] NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_ContentComment] PRIMARY KEY CLUSTERED 
(
	[IDcomment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContentPage]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentPage](
	[IDcontent] [int] NOT NULL,
	[IDpage] [int] NOT NULL,
	[IDuser] [int] NOT NULL,
 CONSTRAINT [PK_SadrzajStranica] PRIMARY KEY CLUSTERED 
(
	[IDcontent] ASC,
	[IDpage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ContentType]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_VrstaSadrzaja] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Contributor]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contributor](
	[IDpage] [int] NOT NULL,
	[IDuser] [int] NOT NULL,
	[IsAuthor] [bit] NOT NULL,
 CONSTRAINT [PK_SudionikStranice] PRIMARY KEY CLUSTERED 
(
	[IDpage] ASC,
	[IDuser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Country]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Drzava] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Country] UNIQUE NONCLUSTERED 
(
	[CountryName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Group]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group](
	[IDgroup] [int] IDENTITY(1,1) NOT NULL,
	[IDgroupType] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[IDgroupOwner] [int] NULL,
 CONSTRAINT [PK_Grupa] PRIMARY KEY CLUSTERED 
(
	[IDgroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GroupType]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_VrstaGrupe] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Location]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[IDlocation] [int] IDENTITY(1,1) NOT NULL,
	[IDcity] [int] NOT NULL,
	[IDlocationType] [int] NOT NULL,
	[TimeChanged] [datetime] NULL,
 CONSTRAINT [PK_Lokacija] PRIMARY KEY CLUSTERED 
(
	[IDlocation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LocationContent]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationContent](
	[IDlocation] [int] NOT NULL,
	[IDcontent] [int] NOT NULL,
	[TimeChanged] [datetime] NULL,
 CONSTRAINT [PK_LokacijaSadrzaj] PRIMARY KEY CLUSTERED 
(
	[IDlocation] ASC,
	[IDcontent] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LocationType]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_VrstaLokacije] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Page]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Page](
	[IDpage] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[IDprivacy] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[PageView] [int] NOT NULL,
	[IDeditor] [int] NULL,
	[TimeChanged] [datetime] NULL,
	[IDauthor] [int] NULL,
 CONSTRAINT [PK_Stranica] PRIMARY KEY CLUSTERED 
(
	[IDpage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PageReview]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PageReview](
	[IDpage] [int] NOT NULL,
	[IDreviewer] [int] NOT NULL,
	[Mark] [smallint] NOT NULL,
	[ShortComment] [nvarchar](200) NULL,
 CONSTRAINT [PK_OcjenaStranice] PRIMARY KEY CLUSTERED 
(
	[IDpage] ASC,
	[IDreviewer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PageTag]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PageTag](
	[IDtag] [int] NOT NULL,
	[IDpage] [int] NOT NULL,
	[TimeChanged] [datetime] NULL,
 CONSTRAINT [PK_TagStranica] PRIMARY KEY CLUSTERED 
(
	[IDtag] ASC,
	[IDpage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Privacy]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Privacy](
	[IDprivacy] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[CanEdit] [bit] NOT NULL,
	[CanShare] [bit] NOT NULL,
 CONSTRAINT [PK_Privatnost] PRIMARY KEY CLUSTERED 
(
	[IDprivacy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tag]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 12.6.2015. 3:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[IDuser] [int] IDENTITY(2,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[IDprofilePic] [int] NULL,
	[IDcityFrom] [int] NOT NULL,
 CONSTRAINT [PK_Korisnik] PRIMARY KEY CLUSTERED 
(
	[IDuser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_User] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[BelongsToGroup]  WITH CHECK ADD  CONSTRAINT [FK_PripadaGrupi_Grupa] FOREIGN KEY([IDgroup])
REFERENCES [dbo].[Group] ([IDgroup])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BelongsToGroup] CHECK CONSTRAINT [FK_PripadaGrupi_Grupa]
GO
ALTER TABLE [dbo].[BelongsToGroup]  WITH CHECK ADD  CONSTRAINT [FK_PripadaGrupi_Korisnik] FOREIGN KEY([IDuser])
REFERENCES [dbo].[User] ([IDuser])
GO
ALTER TABLE [dbo].[BelongsToGroup] CHECK CONSTRAINT [FK_PripadaGrupi_Korisnik]
GO
ALTER TABLE [dbo].[City]  WITH CHECK ADD  CONSTRAINT [FK_Grad_Drzava] FOREIGN KEY([IDcountry])
REFERENCES [dbo].[Country] ([ID])
GO
ALTER TABLE [dbo].[City] CHECK CONSTRAINT [FK_Grad_Drzava]
GO
ALTER TABLE [dbo].[Content]  WITH CHECK ADD  CONSTRAINT [FK_Content_User] FOREIGN KEY([IDauthor])
REFERENCES [dbo].[User] ([IDuser])
GO
ALTER TABLE [dbo].[Content] CHECK CONSTRAINT [FK_Content_User]
GO
ALTER TABLE [dbo].[Content]  WITH CHECK ADD  CONSTRAINT [FK_Sadrzaj_Korisnik] FOREIGN KEY([IDeditor])
REFERENCES [dbo].[User] ([IDuser])
GO
ALTER TABLE [dbo].[Content] CHECK CONSTRAINT [FK_Sadrzaj_Korisnik]
GO
ALTER TABLE [dbo].[Content]  WITH CHECK ADD  CONSTRAINT [FK_Sadrzaj_VrstaSadrzaja] FOREIGN KEY([IDcontentType])
REFERENCES [dbo].[ContentType] ([ID])
GO
ALTER TABLE [dbo].[Content] CHECK CONSTRAINT [FK_Sadrzaj_VrstaSadrzaja]
GO
ALTER TABLE [dbo].[ContentComment]  WITH CHECK ADD  CONSTRAINT [FK_ContentComment_Content] FOREIGN KEY([IDcontent])
REFERENCES [dbo].[Content] ([IDcontent])
GO
ALTER TABLE [dbo].[ContentComment] CHECK CONSTRAINT [FK_ContentComment_Content]
GO
ALTER TABLE [dbo].[ContentComment]  WITH CHECK ADD  CONSTRAINT [FK_ContentComment_User] FOREIGN KEY([IDuser])
REFERENCES [dbo].[User] ([IDuser])
GO
ALTER TABLE [dbo].[ContentComment] CHECK CONSTRAINT [FK_ContentComment_User]
GO
ALTER TABLE [dbo].[ContentPage]  WITH CHECK ADD  CONSTRAINT [FK_SadrzajStranica_Korisnik] FOREIGN KEY([IDuser])
REFERENCES [dbo].[User] ([IDuser])
GO
ALTER TABLE [dbo].[ContentPage] CHECK CONSTRAINT [FK_SadrzajStranica_Korisnik]
GO
ALTER TABLE [dbo].[ContentPage]  WITH CHECK ADD  CONSTRAINT [FK_SadrzajStranica_Sadrzaj] FOREIGN KEY([IDcontent])
REFERENCES [dbo].[Content] ([IDcontent])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContentPage] CHECK CONSTRAINT [FK_SadrzajStranica_Sadrzaj]
GO
ALTER TABLE [dbo].[ContentPage]  WITH CHECK ADD  CONSTRAINT [FK_SadrzajStranica_Stranica] FOREIGN KEY([IDpage])
REFERENCES [dbo].[Page] ([IDpage])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContentPage] CHECK CONSTRAINT [FK_SadrzajStranica_Stranica]
GO
ALTER TABLE [dbo].[Contributor]  WITH CHECK ADD  CONSTRAINT [FK_SudionikStranice_Korisnik] FOREIGN KEY([IDuser])
REFERENCES [dbo].[User] ([IDuser])
GO
ALTER TABLE [dbo].[Contributor] CHECK CONSTRAINT [FK_SudionikStranice_Korisnik]
GO
ALTER TABLE [dbo].[Contributor]  WITH CHECK ADD  CONSTRAINT [FK_SudionikStranice_Stranica] FOREIGN KEY([IDpage])
REFERENCES [dbo].[Page] ([IDpage])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Contributor] CHECK CONSTRAINT [FK_SudionikStranice_Stranica]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Grupa_Korisnik] FOREIGN KEY([IDgroupOwner])
REFERENCES [dbo].[User] ([IDuser])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Grupa_Korisnik]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Grupa_VrstaGrupe] FOREIGN KEY([IDgroupType])
REFERENCES [dbo].[GroupType] ([ID])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Grupa_VrstaGrupe]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Lokacija_Grad] FOREIGN KEY([IDcity])
REFERENCES [dbo].[City] ([IDcity])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Lokacija_Grad]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Lokacija_VrstaLokacije] FOREIGN KEY([IDlocationType])
REFERENCES [dbo].[LocationType] ([ID])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Lokacija_VrstaLokacije]
GO
ALTER TABLE [dbo].[LocationContent]  WITH CHECK ADD  CONSTRAINT [FK_LocationContent_City] FOREIGN KEY([IDlocation])
REFERENCES [dbo].[City] ([IDcity])
GO
ALTER TABLE [dbo].[LocationContent] CHECK CONSTRAINT [FK_LocationContent_City]
GO
ALTER TABLE [dbo].[LocationContent]  WITH CHECK ADD  CONSTRAINT [FK_LokacijaSadrzaj_Lokacija] FOREIGN KEY([IDlocation])
REFERENCES [dbo].[Location] ([IDlocation])
GO
ALTER TABLE [dbo].[LocationContent] CHECK CONSTRAINT [FK_LokacijaSadrzaj_Lokacija]
GO
ALTER TABLE [dbo].[LocationContent]  WITH CHECK ADD  CONSTRAINT [FK_LokacijaSadrzaj_Sadrzaj] FOREIGN KEY([IDcontent])
REFERENCES [dbo].[Content] ([IDcontent])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LocationContent] CHECK CONSTRAINT [FK_LokacijaSadrzaj_Sadrzaj]
GO
ALTER TABLE [dbo].[Page]  WITH CHECK ADD  CONSTRAINT [FK_Page_User] FOREIGN KEY([IDauthor])
REFERENCES [dbo].[User] ([IDuser])
GO
ALTER TABLE [dbo].[Page] CHECK CONSTRAINT [FK_Page_User]
GO
ALTER TABLE [dbo].[Page]  WITH CHECK ADD  CONSTRAINT [FK_Stranica_Korisnik] FOREIGN KEY([IDeditor])
REFERENCES [dbo].[User] ([IDuser])
GO
ALTER TABLE [dbo].[Page] CHECK CONSTRAINT [FK_Stranica_Korisnik]
GO
ALTER TABLE [dbo].[Page]  WITH CHECK ADD  CONSTRAINT [FK_Stranica_Privatnost] FOREIGN KEY([IDprivacy])
REFERENCES [dbo].[Privacy] ([IDprivacy])
GO
ALTER TABLE [dbo].[Page] CHECK CONSTRAINT [FK_Stranica_Privatnost]
GO
ALTER TABLE [dbo].[PageReview]  WITH CHECK ADD  CONSTRAINT [FK_OcjenaStranice_Korisnik] FOREIGN KEY([IDreviewer])
REFERENCES [dbo].[User] ([IDuser])
GO
ALTER TABLE [dbo].[PageReview] CHECK CONSTRAINT [FK_OcjenaStranice_Korisnik]
GO
ALTER TABLE [dbo].[PageReview]  WITH CHECK ADD  CONSTRAINT [FK_OcjenaStranice_Stranica] FOREIGN KEY([IDpage])
REFERENCES [dbo].[Page] ([IDpage])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PageReview] CHECK CONSTRAINT [FK_OcjenaStranice_Stranica]
GO
ALTER TABLE [dbo].[PageTag]  WITH CHECK ADD  CONSTRAINT [FK_TagStranica_Stranica] FOREIGN KEY([IDpage])
REFERENCES [dbo].[Page] ([IDpage])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PageTag] CHECK CONSTRAINT [FK_TagStranica_Stranica]
GO
ALTER TABLE [dbo].[PageTag]  WITH CHECK ADD  CONSTRAINT [FK_TagStranica_Tag] FOREIGN KEY([IDtag])
REFERENCES [dbo].[Tag] ([ID])
GO
ALTER TABLE [dbo].[PageTag] CHECK CONSTRAINT [FK_TagStranica_Tag]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_Korisnik_Grad] FOREIGN KEY([IDcityFrom])
REFERENCES [dbo].[City] ([IDcity])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_Korisnik_Grad]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_Korisnik_Sadrzaj] FOREIGN KEY([IDprofilePic])
REFERENCES [dbo].[Content] ([IDcontent])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_Korisnik_Sadrzaj]
GO
