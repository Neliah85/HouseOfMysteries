-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Feb 26. 10:03
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

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `csapatok`
--
ALTER TABLE `csapatok`
  ADD PRIMARY KEY (`CsapatID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
