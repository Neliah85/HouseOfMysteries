-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Feb 01. 15:33
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `rejtelyekhaza`
--
CREATE DATABASE IF NOT EXISTS `rejtelyekhaza` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `rejtelyekhaza`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `admin`
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
-- Tábla szerkezet ehhez a táblához `csapatok`
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
-- Tábla szerkezet ehhez a táblához `szoba1`
--

CREATE TABLE `szoba1` (
  `Foglalt_ip` datetime NOT NULL,
  `AdminID` char(36) NOT NULL,
  `CsapatID` char(36) NOT NULL,
  `Zar` timestamp NULL DEFAULT NULL,
  `Nyit` timestamp NULL DEFAULT NULL,
  `Komment` char(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `szoba2`
--

CREATE TABLE `szoba2` (
  `Foglalt_ip` datetime NOT NULL,
  `AdminID` char(36) NOT NULL,
  `CsapatID` char(36) NOT NULL,
  `Zar` timestamp NULL DEFAULT NULL,
  `Nyit` timestamp NULL DEFAULT NULL,
  `Komment` char(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `admin`
--
ALTER TABLE `admin`
  ADD PRIMARY KEY (`AdminID`);

--
-- A tábla indexei `csapatok`
--
ALTER TABLE `csapatok`
  ADD PRIMARY KEY (`CsapatID`);

--
-- A tábla indexei `szoba1`
--
ALTER TABLE `szoba1`
  ADD KEY `AdminID` (`AdminID`),
  ADD KEY `CsapatID` (`CsapatID`);

--
-- A tábla indexei `szoba2`
--
ALTER TABLE `szoba2`
  ADD KEY `AdminID` (`AdminID`,`CsapatID`),
  ADD KEY `CsapatID` (`CsapatID`);

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `szoba1`
--
ALTER TABLE `szoba1`
  ADD CONSTRAINT `szoba1_ibfk_1` FOREIGN KEY (`CsapatID`) REFERENCES `csapatok` (`CsapatID`),
  ADD CONSTRAINT `szoba1_ibfk_2` FOREIGN KEY (`AdminID`) REFERENCES `admin` (`AdminID`);

--
-- Megkötések a táblához `szoba2`
--
ALTER TABLE `szoba2`
  ADD CONSTRAINT `szoba2_ibfk_1` FOREIGN KEY (`CsapatID`) REFERENCES `csapatok` (`CsapatID`),
  ADD CONSTRAINT `szoba2_ibfk_2` FOREIGN KEY (`AdminID`) REFERENCES `admin` (`AdminID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
