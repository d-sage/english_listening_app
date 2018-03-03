-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 03, 2018 at 10:58 PM
-- Server version: 10.1.26-MariaDB
-- PHP Version: 7.1.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `daricsag_ela`
--

-- --------------------------------------------------------

--
-- Table structure for table `countries`
--

CREATE TABLE `countries` (
  `cid` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `country_grade_relationship`
--

CREATE TABLE `country_grade_relationship` (
  `cid` varchar(30) NOT NULL,
  `gid` tinyint(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `country_grade_topic_relation`
--

CREATE TABLE `country_grade_topic_relation` (
  `cid` varchar(30) NOT NULL,
  `gid` tinyint(4) NOT NULL,
  `tid` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `credentials`
--

CREATE TABLE `credentials` (
  `username` varchar(20) NOT NULL,
  `salt` varchar(80) NOT NULL,
  `hash` varchar(241) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `credentials`
--

INSERT INTO `credentials` (`username`, `salt`, `hash`) VALUES
('AdminELA', '81126147209236142226824328583818221323126120108971201223861621451535121312919915', '2442022411122349917696223246343111622219816422031511843150228361812417717097232104806116156106715981331392471161472361447076896854076189109105204170922261282018181126147209236142226824328583818221323126120108971201223861621451535121312919915');

-- --------------------------------------------------------

--
-- Table structure for table `grades`
--

CREATE TABLE `grades` (
  `gid` tinyint(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `lessons`
--

CREATE TABLE `lessons` (
  `cid` varchar(30) NOT NULL,
  `gid` tinyint(4) NOT NULL,
  `tid` varchar(30) NOT NULL,
  `lid` varchar(30) NOT NULL,
  `text` varchar(500) NOT NULL,
  `path` varchar(260) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `session`
--

CREATE TABLE `session` (
  `id` varchar(80) NOT NULL,
  `time` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `topics`
--

CREATE TABLE `topics` (
  `tid` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `countries`
--
ALTER TABLE `countries`
  ADD PRIMARY KEY (`cid`);

--
-- Indexes for table `country_grade_relationship`
--
ALTER TABLE `country_grade_relationship`
  ADD PRIMARY KEY (`cid`,`gid`),
  ADD KEY `gid` (`gid`);

--
-- Indexes for table `country_grade_topic_relation`
--
ALTER TABLE `country_grade_topic_relation`
  ADD PRIMARY KEY (`cid`,`gid`,`tid`),
  ADD KEY `country_grade_topic_relation_ibfk_2` (`tid`);

--
-- Indexes for table `credentials`
--
ALTER TABLE `credentials`
  ADD PRIMARY KEY (`username`,`salt`,`hash`);

--
-- Indexes for table `grades`
--
ALTER TABLE `grades`
  ADD PRIMARY KEY (`gid`);

--
-- Indexes for table `lessons`
--
ALTER TABLE `lessons`
  ADD PRIMARY KEY (`cid`,`gid`,`tid`,`lid`);

--
-- Indexes for table `session`
--
ALTER TABLE `session`
  ADD PRIMARY KEY (`id`,`time`);

--
-- Indexes for table `topics`
--
ALTER TABLE `topics`
  ADD PRIMARY KEY (`tid`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `country_grade_relationship`
--
ALTER TABLE `country_grade_relationship`
  ADD CONSTRAINT `country_grade_relationship_ibfk_1` FOREIGN KEY (`cid`) REFERENCES `countries` (`cid`),
  ADD CONSTRAINT `country_grade_relationship_ibfk_2` FOREIGN KEY (`gid`) REFERENCES `grades` (`gid`);

--
-- Constraints for table `country_grade_topic_relation`
--
ALTER TABLE `country_grade_topic_relation`
  ADD CONSTRAINT `country_grade_topic_relation_ibfk_1` FOREIGN KEY (`cid`,`gid`) REFERENCES `country_grade_relationship` (`cid`, `gid`),
  ADD CONSTRAINT `country_grade_topic_relation_ibfk_2` FOREIGN KEY (`tid`) REFERENCES `topics` (`tid`);

--
-- Constraints for table `lessons`
--
ALTER TABLE `lessons`
  ADD CONSTRAINT `lessons_ibfk_1` FOREIGN KEY (`cid`,`gid`,`tid`) REFERENCES `country_grade_topic_relation` (`cid`, `gid`, `tid`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
