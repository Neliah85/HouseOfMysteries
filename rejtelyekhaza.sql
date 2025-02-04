-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- G�p: 127.0.0.1
-- L�trehoz�s ideje: 2025. Feb 04. 14:07
-- Kiszolg�l� verzi�ja: 10.4.32-MariaDB
-- PHP verzi�: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatb�zis: `rejtelyekhaza`
--
CREATE DATABASE IF NOT EXISTS `rejtelyekhaza` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `rejtelyekhaza`;

-- --------------------------------------------------------

--
-- T�bla szerkezet ehhez a t�bl�hoz `admin`
--

CREATE TABLE `admin` (
  `AdminID` char(36) NOT NULL,
  `Nev` char(60) DEFAULT NULL,
  `Szint` int(1) DEFAULT NULL,
  `SALT` varchar(64) NOT NULL,
  `HASH` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- T�bla szerkezet ehhez a t�bl�hoz `csapatok`
--

CREATE TABLE `csapatok` (
  `CsapatID` char(36) NOT NULL,
  `Nev` char(60) DEFAULT NULL,
  `CsKapitany` char(60) DEFAULT NULL,
  `email` char(100) DEFAULT NULL,
  `telefonszam` int(11) DEFAULT NULL,
  `SALT` varchar(64) DEFAULT NULL,
  `HASH` varchar(64) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- T�bla szerkezet ehhez a t�bl�hoz `szoba1`
--

CREATE TABLE `szoba1` (
  `Foglalt_ip` datetime NOT NULL,
  `AdminID` char(36) DEFAULT NULL,
  `CsapatID` char(36) DEFAULT NULL,
  `Zar` timestamp NULL DEFAULT NULL,
  `Nyit` timestamp NULL DEFAULT NULL,
  `Komment` char(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- T�bla szerkezet ehhez a t�bl�hoz `szoba2`
--

CREATE TABLE `szoba2` (
  `Foglalt_ip` datetime NOT NULL,
  `AdminID` char(36) DEFAULT NULL,
  `CsapatID` char(36) DEFAULT NULL,
  `Zar` timestamp NULL DEFAULT NULL,
  `Nyit` timestamp NULL DEFAULT NULL,
  `Komment` char(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- A n�zet helyettes szerkezete `versenyeredmenyek_szoba1`
-- (L�sd al�bb az aktu�lis n�zetet)
--
CREATE TABLE `versenyeredmenyek_szoba1` (
`csapat_nev` char(60)
,`kijutasi_ido` time
,`helyezes` bigint(21)
);

-- --------------------------------------------------------

--
-- A n�zet helyettes szerkezete `versenyeredmenyek_szoba2`
-- (L�sd al�bb az aktu�lis n�zetet)
--
CREATE TABLE `versenyeredmenyek_szoba2` (
`csapat_nev` char(60)
,`kijutasi_ido` time
,`helyezes` bigint(21)
);

-- --------------------------------------------------------

--
-- N�zet szerkezete `versenyeredmenyek_szoba1`
--
DROP TABLE IF EXISTS `versenyeredmenyek_szoba1`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `versenyeredmenyek_szoba1`  AS SELECT `csapatok`.`Nev` AS `csapat_nev`, timediff(`szoba1`.`Zar`,`szoba1`.`Nyit`) AS `kijutasi_ido`, rank() over ( order by timediff(`szoba1`.`Zar`,`szoba1`.`Nyit`)) AS `helyezes` FROM (`szoba1` join `csapatok` on(`szoba1`.`CsapatID` = `csapatok`.`CsapatID`)) WHERE `szoba1`.`Zar` is not null AND `szoba1`.`Nyit` is not null ;

-- --------------------------------------------------------

--
-- N�zet szerkezete `versenyeredmenyek_szoba2`
--
DROP TABLE IF EXISTS `versenyeredmenyek_szoba2`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `versenyeredmenyek_szoba2`  AS SELECT `csapatok`.`Nev` AS `csapat_nev`, timediff(`szoba2`.`Zar`,`szoba2`.`Nyit`) AS `kijutasi_ido`, rank() over ( order by timediff(`szoba2`.`Zar`,`szoba2`.`Nyit`)) AS `helyezes` FROM (`szoba2` join `csapatok` on(`szoba2`.`CsapatID` = `csapatok`.`CsapatID`)) WHERE `szoba2`.`Zar` is not null AND `szoba2`.`Nyit` is not null ;

--
-- Indexek a ki�rt t�bl�khoz
--

--
-- A t�bla indexei `admin`
--
ALTER TABLE `admin`
  ADD PRIMARY KEY (`AdminID`);

--
-- A t�bla indexei `csapatok`
--
ALTER TABLE `csapatok`
  ADD PRIMARY KEY (`CsapatID`);

--
-- A t�bla indexei `szoba1`
--
ALTER TABLE `szoba1`
  ADD KEY `AdminID` (`AdminID`),
  ADD KEY `CsapatID` (`CsapatID`);

--
-- A t�bla indexei `szoba2`
--
ALTER TABLE `szoba2`
  ADD KEY `AdminID` (`AdminID`,`CsapatID`),
  ADD KEY `CsapatID` (`CsapatID`);

--
-- Megk�t�sek a ki�rt t�bl�khoz
--

--
-- Megk�t�sek a t�bl�hoz `szoba1`
--
ALTER TABLE `szoba1`
  ADD CONSTRAINT `szoba1_ibfk_1` FOREIGN KEY (`CsapatID`) REFERENCES `csapatok` (`CsapatID`),
  ADD CONSTRAINT `szoba1_ibfk_2` FOREIGN KEY (`AdminID`) REFERENCES `admin` (`AdminID`);

--
-- Megk�t�sek a t�bl�hoz `szoba2`
--
ALTER TABLE `szoba2`
  ADD CONSTRAINT `szoba2_ibfk_1` FOREIGN KEY (`CsapatID`) REFERENCES `csapatok` (`CsapatID`),
  ADD CONSTRAINT `szoba2_ibfk_2` FOREIGN KEY (`AdminID`) REFERENCES `admin` (`AdminID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
