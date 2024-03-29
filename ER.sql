USE [master]
GO
/****** Object:  Database [Billing]    Script Date: 19/07/2019 16:15:42 ******/
CREATE DATABASE [Billing]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Billing', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Billing.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Billing_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Billing_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Billing] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Billing].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Billing] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Billing] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Billing] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Billing] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Billing] SET ARITHABORT OFF 
GO
ALTER DATABASE [Billing] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Billing] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Billing] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Billing] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Billing] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Billing] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Billing] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Billing] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Billing] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Billing] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Billing] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Billing] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Billing] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Billing] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Billing] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Billing] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Billing] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Billing] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Billing] SET  MULTI_USER 
GO
ALTER DATABASE [Billing] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Billing] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Billing] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Billing] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Billing] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Billing] SET QUERY_STORE = OFF
GO
USE [Billing]
GO
/****** Object:  Table [dbo].[SAP]    Script Date: 19/07/2019 16:15:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SAP](
	[Billing_Date] [date] NULL,
	[Bill_To_Party] [nvarchar](250) NULL,
	[Bill_To_Party_Description] [nvarchar](250) NULL,
	[Ref_Document] [nvarchar](250) NOT NULL,
	[HN] [nvarchar](250) NULL,
	[Patient_Name] [nvarchar](250) NULL,
	[Net_revenue] [float] NULL,
	[Cashier_name] [nvarchar](250) NULL,
	[Collector_Name] [nvarchar](250) NULL,
	[Cost_center] [nvarchar](1) NULL,
 CONSTRAINT [PK_SAP_1] PRIMARY KEY CLUSTERED 
(
	[Ref_Document] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Imed]    Script Date: 19/07/2019 16:15:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Imed](
	[Receive_Date] [date] NULL,
	[HN] [varchar](250) NULL,
	[Receipt_No] [varchar](250) NOT NULL,
	[Name] [varchar](250) NULL,
	[Net] [float] NULL,
	[payer_name] [varchar](250) NULL,
	[regis_name] [varchar](250) NULL,
	[welfare_name] [varchar](250) NULL,
	[AR] [varchar](250) NULL,
	[Pending_delivery] [bit] NULL,
	[Date_Scan] [datetime] NULL,
 CONSTRAINT [PK_Imed] PRIMARY KEY CLUSTERED 
(
	[Receipt_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[chk_bill]    Script Date: 19/07/2019 16:15:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[chk_bill]
AS
SELECT        dbo.Imed.Receive_Date, dbo.Imed.HN, dbo.Imed.Receipt_No, dbo.Imed.Name, dbo.Imed.Net, dbo.Imed.payer_name, dbo.Imed.regis_name, dbo.Imed.welfare_name, dbo.Imed.AR, dbo.Imed.Pending_delivery, 
                         dbo.SAP.HN AS SAP_HN, dbo.Imed.Date_Scan
FROM            dbo.Imed LEFT OUTER JOIN
                         dbo.SAP ON dbo.Imed.Receipt_No = dbo.SAP.Ref_Document
GO
ALTER TABLE [dbo].[Imed] ADD  CONSTRAINT [DF_Imed_Pending_delivery]  DEFAULT ((0)) FOR [Pending_delivery]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[43] 4[28] 2[15] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Imed"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 201
               Right = 217
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "SAP"
            Begin Extent = 
               Top = 6
               Left = 255
               Bottom = 196
               Right = 475
            End
            DisplayFlags = 280
            TopColumn = 3
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'chk_bill'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'chk_bill'
GO
USE [master]
GO
ALTER DATABASE [Billing] SET  READ_WRITE 
GO
