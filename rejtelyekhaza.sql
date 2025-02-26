-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Feb 26. 10:15
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

--
-- A tábla adatainak kiíratása `admin`
--

INSERT INTO `admin` (`AdminID`, `Nev`, `Szint`, `SALT`, `HASH`) VALUES
('a1b2c3d4-e5f6-7890-abcd-ef1234567890', 'Kovács Béla', 1, 'randomsalt1', 'randomhash1'),
('b2c3d4e5-f678-9012-bcde-f12345678901', 'Szabó Anna', 2, 'randomsalt2', 'randomhash2'),
('c3d4e5f6-7890-1234-cdef-123456789012', 'Nagy Péter', 1, 'randomsalt3', 'randomhash3'),
('d4e5f678-9012-3456-def1-234567890123', 'Tóth László', 3, 'randomsalt4', 'randomhash4'),
('e5f67890-1234-5678-ef12-345678901234', 'Varga Zoltán', 2, 'randomsalt5', 'randomhash5');

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

--
-- A tábla adatainak kiíratása `csapatok`
--

INSERT INTO `csapatok` (`CsapatID`, `Nev`, `CsKapitany`, `email`, `telefonszam`, `SALT`, `HASH`) VALUES
('567890ab-cdef-1234-5678-9abcdef01234', 'Harcosok', 'Szabó Dániel', 'daniel.szabo@example.com', 2147483647, 'teamsalt5', 'teamhash5'),
('c1d2e3f4-5678-90ab-cdef-123456789abc', 'Győztesek', 'Kiss Gergő', 'gergo.kiss@example.com', 214748364, 'teamsalt1', 'teamhash1'),
('d2e3f456-7890-abcd-ef12-3456789abcde', 'Villámok', 'Horváth Réka', 'reka.horvath@example.com', 123456778, 'teamsalt2', 'teamhash2'),
('e3f45678-90ab-cdef-1234-56789abcdef0', 'Tigrisek', 'Balogh Ádám', 'adam.balogh@example.com', 7483647, 'teamsalt3', 'teamhash3'),
('f4567890-abcd-ef12-3456-789abcdef012', 'Mágusok', 'Kovács Eszter', 'eszter.kovacs@example.com', 214748647, 'teamsalt4', 'teamhash4');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `szoba1`
--

CREATE TABLE `szoba1` (
  `Foglalt_ip` datetime NOT NULL,
  `AdminID` char(36) DEFAULT NULL,
  `CsapatID` char(36) DEFAULT NULL,
  `Zar` timestamp NULL DEFAULT NULL,
  `Nyit` timestamp NULL DEFAULT NULL,
  `Komment` char(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `szoba1`
--

INSERT INTO `szoba1` (`Foglalt_ip`, `AdminID`, `CsapatID`, `Zar`, `Nyit`, `Komment`) VALUES
('2025-02-10 09:00:00', 'a1b2c3d4-e5f6-7890-abcd-ef1234567890', 'c1d2e3f4-5678-90ab-cdef-123456789abc', '2025-02-10 08:00:00', '2025-02-10 09:30:00', 'Nagyon gyorsak voltak!'),
('2025-02-11 10:30:00', 'b2c3d4e5-f678-9012-bcde-f12345678901', 'd2e3f456-7890-abcd-ef12-3456789abcde', '2025-02-11 09:30:00', '2025-02-11 11:00:00', 'Nehézségek a rejtélyeknél'),
('2025-02-12 12:00:00', 'c3d4e5f6-7890-1234-cdef-123456789012', 'e3f45678-90ab-cdef-1234-56789abcdef0', '2025-02-12 11:00:00', '2025-02-12 12:30:00', 'Szinte rekordidő!'),
('2025-02-13 13:30:00', 'd4e5f678-9012-3456-def1-234567890123', 'f4567890-abcd-ef12-3456-789abcdef012', '2025-02-13 12:30:00', '2025-02-13 14:00:00', 'Néhány extra segítséget igényeltek.'),
('2025-02-14 15:00:00', 'e5f67890-1234-5678-ef12-345678901234', '567890ab-cdef-1234-5678-9abcdef01234', '2025-02-14 14:00:00', '2025-02-14 15:30:00', 'Nagyon kreatív csapat!');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `szoba2`
--

CREATE TABLE `szoba2` (
  `Foglalt_ip` datetime NOT NULL,
  `AdminID` char(36) DEFAULT NULL,
  `CsapatID` char(36) DEFAULT NULL,
  `Zar` timestamp NULL DEFAULT NULL,
  `Nyit` timestamp NULL DEFAULT NULL,
  `Komment` char(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `szoba2`
--

INSERT INTO `szoba2` (`Foglalt_ip`, `AdminID`, `CsapatID`, `Zar`, `Nyit`, `Komment`) VALUES
('2025-02-15 09:00:00', 'a1b2c3d4-e5f6-7890-abcd-ef1234567890', 'c1d2e3f4-5678-90ab-cdef-123456789abc', '2025-02-15 08:00:00', '2025-02-15 09:30:00', 'Nagyon élvezték a játékot!'),
('2025-02-16 10:30:00', 'b2c3d4e5-f678-9012-bcde-f12345678901', 'd2e3f456-7890-abcd-ef12-3456789abcde', '2025-02-16 09:30:00', '2025-02-16 11:00:00', 'Izgalmas fordulatok voltak!'),
('2025-02-17 12:00:00', 'c3d4e5f6-7890-1234-cdef-123456789012', 'e3f45678-90ab-cdef-1234-56789abcdef0', '2025-02-17 11:00:00', '2025-02-17 12:30:00', 'Remek logikai készségek!'),
('2025-02-18 13:30:00', 'd4e5f678-9012-3456-def1-234567890123', 'f4567890-abcd-ef12-3456-789abcdef012', '2025-02-18 12:30:00', '2025-02-18 14:00:00', 'Nagyon lelkesek voltak!'),
('2025-02-19 15:00:00', 'e5f67890-1234-5678-ef12-345678901234', '567890ab-cdef-1234-5678-9abcdef01234', '2025-02-19 14:00:00', '2025-02-19 15:30:00', 'Egy új rekord a szobában!');

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
