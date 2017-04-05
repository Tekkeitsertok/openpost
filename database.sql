SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

--
-- Database: `openpost`
--

-- --------------------------------------------------------

--
-- Table structure for table `Authors`
--

CREATE TABLE `Authors` (
  `Id` varchar(22) COLLATE utf8mb4_bin NOT NULL,
  `AvatarUrl` longtext COLLATE utf8mb4_bin,
  `DisplayName` longtext COLLATE utf8mb4_bin,
  `Email` longtext COLLATE utf8mb4_bin,
  `PlatformId` longtext COLLATE utf8mb4_bin,
  `RowVersion` longblob,
  `SourcePlatformId` varchar(22) COLLATE utf8mb4_bin DEFAULT NULL,
  `TokenId` varchar(22) COLLATE utf8mb4_bin DEFAULT NULL,
  `IsAnonymous` bit(1) NOT NULL DEFAULT b'0',
  `WebSite` longtext COLLATE utf8mb4_bin
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- --------------------------------------------------------

--
-- Table structure for table `Comments`
--

CREATE TABLE `Comments` (
  `Id` varchar(22) COLLATE utf8mb4_bin NOT NULL,
  `AuthorId` varchar(22) COLLATE utf8mb4_bin DEFAULT NULL,
  `Content` longtext COLLATE utf8mb4_bin,
  `Depth` tinyint(3) UNSIGNED NOT NULL,
  `PageId` varchar(22) COLLATE utf8mb4_bin DEFAULT NULL,
  `ParentId` varchar(22) COLLATE utf8mb4_bin DEFAULT NULL,
  `PostDate` datetime(6) NOT NULL,
  `RowVersion` longblob,
  `Title` longtext COLLATE utf8mb4_bin
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- --------------------------------------------------------

--
-- Table structure for table `Pages`
--

CREATE TABLE `Pages` (
  `Id` varchar(22) COLLATE utf8mb4_bin NOT NULL,
  `CommentsCount` int(11) NOT NULL,
  `PublicIdentifier` longtext COLLATE utf8mb4_bin NOT NULL,
  `SourcePlatformId` varchar(22) COLLATE utf8mb4_bin NOT NULL,
  `AllowAnonymousComments` bit(1) NOT NULL DEFAULT b'0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- --------------------------------------------------------

--
-- Table structure for table `Platforms`
--

CREATE TABLE `Platforms` (
  `Id` varchar(22) COLLATE utf8mb4_bin NOT NULL,
  `Name` longtext COLLATE utf8mb4_bin,
  `ProviderApi` longtext COLLATE utf8mb4_bin,
  `ProviderAuthKey` longtext COLLATE utf8mb4_bin,
  `ProviderAuthPassword` longtext COLLATE utf8mb4_bin,
  `RowVersion` longblob
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- --------------------------------------------------------

--
-- Table structure for table `__EFMigrationsHistory`
--

CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(95) COLLATE utf8mb4_bin NOT NULL,
  `ProductVersion` varchar(32) COLLATE utf8mb4_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

--
-- Dumping data for table `__EFMigrationsHistory`
--

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20170401161021_Initial', '1.1.1'),
('20170405171208_AnonymousComments', '1.1.1');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `Authors`
--
ALTER TABLE `Authors`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Authors_SourcePlatformId` (`SourcePlatformId`);

--
-- Indexes for table `Comments`
--
ALTER TABLE `Comments`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Comments_AuthorId` (`AuthorId`),
  ADD KEY `IX_Comments_PageId` (`PageId`),
  ADD KEY `IX_Comments_ParentId` (`ParentId`);

--
-- Indexes for table `Pages`
--
ALTER TABLE `Pages`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Pages_SourcePlatformId` (`SourcePlatformId`);

--
-- Indexes for table `Platforms`
--
ALTER TABLE `Platforms`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `__EFMigrationsHistory`
--
ALTER TABLE `__EFMigrationsHistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `Authors`
--
ALTER TABLE `Authors`
  ADD CONSTRAINT `FK_Authors_Platforms_SourcePlatformId` FOREIGN KEY (`SourcePlatformId`) REFERENCES `Platforms` (`Id`) ON DELETE NO ACTION;

--
-- Constraints for table `Comments`
--
ALTER TABLE `Comments`
  ADD CONSTRAINT `FK_Comments_Authors_AuthorId` FOREIGN KEY (`AuthorId`) REFERENCES `Authors` (`Id`) ON DELETE NO ACTION,
  ADD CONSTRAINT `FK_Comments_Comments_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `Comments` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Comments_Pages_PageId` FOREIGN KEY (`PageId`) REFERENCES `Pages` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `Pages`
--
ALTER TABLE `Pages`
  ADD CONSTRAINT `FK_Pages_Platforms_SourcePlatformId` FOREIGN KEY (`SourcePlatformId`) REFERENCES `Platforms` (`Id`) ON DELETE CASCADE;
