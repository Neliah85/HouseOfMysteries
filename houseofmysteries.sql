-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Már 24. 12:34
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
-- Adatbázis: `houseofmysteries`
--
CREATE DATABASE IF NOT EXISTS `houseofmysteries` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `houseofmysteries`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `booking`
--

CREATE TABLE `booking` (
  `bookingId` int(11) NOT NULL,
  `bookingDate` datetime DEFAULT NULL,
  `roomId` int(11) DEFAULT NULL,
  `teamId` int(11) DEFAULT NULL,
  `result` time DEFAULT NULL,
  `isAvailable` tinyint(1) DEFAULT NULL,
  `comment` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `booking`
--

INSERT INTO `booking` (`bookingId`, `bookingDate`, `roomId`, `teamId`, `result`, `isAvailable`, `comment`) VALUES
(1, '2025-03-01 14:00:00', 1, 1, NULL, 0, 'Jó csapatmunka!'),
(2, '2025-03-02 15:30:00', 1, 1, '00:50:10', 0, 'Éppenhogy kijutottak.'),
(3, '2025-03-03 12:45:00', 1, 1, '00:40:25', 0, 'Új rekord!'),
(4, '2025-03-04 16:10:00', 2, 1, '00:55:15', 0, 'Sikeresen teljesítették.'),
(5, '2025-03-05 18:20:00', 2, 1, '00:48:50', 0, 'Nagyon ügyesek voltak.'),
(6, '2025-03-06 14:30:00', 3, 1, '00:42:00', 0, 'Gyors és hatékony csapat.'),
(7, '2025-03-07 16:00:00', 3, 1, '00:39:45', 0, 'Kiemelkedő teljesítmény.'),
(8, '2025-03-08 17:45:00', 3, 1, '00:44:15', 0, 'Majdnem rekord.'),
(9, '2025-03-09 15:50:00', 4, 1, '00:50:30', 0, 'Izgalmas befejezés.'),
(10, '2025-03-10 13:15:00', 4, 1, '00:46:20', 0, 'Nagyon ügyesen játszottak.'),
(11, '2025-03-11 12:00:00', 4, 1, '00:52:10', 0, 'Egy hajszálon múlt.'),
(12, '2025-03-12 11:00:00', 5, 1, '00:39:10', 0, 'Új csúcsidő!'),
(13, '2025-03-13 10:30:00', 5, 1, '00:41:50', 0, 'Közel voltak a rekordhoz.'),
(14, '2025-03-14 09:45:00', 5, 1, '00:43:25', 0, 'Nagyszerű teljesítmény.'),
(15, '2025-03-15 14:10:00', 6, 1, '00:49:30', 0, 'Nagyon izgalmas játék volt.'),
(16, '2025-03-16 15:45:00', 6, 1, '00:47:20', 0, 'Szinte rekordot döntöttek.'),
(17, '2025-03-17 16:30:00', 6, 1, '00:51:45', 0, 'Egy hajszálon múlt.'),
(18, '2025-03-18 17:00:00', 7, 1, '00:40:10', 0, 'Villámgyors csapatmunka!'),
(19, '2025-03-19 14:25:00', 7, 1, '00:43:50', 0, 'Nagyszerű teljesítmény.'),
(20, '2025-03-20 18:10:00', 7, 1, '00:45:00', 0, 'Éppenhogy sikerült kijutni.'),
(21, '2025-03-21 12:40:00', 8, 1, '00:39:20', 0, 'Új szobarekord!'),
(22, '2025-03-22 11:50:00', 8, 1, '00:42:15', 0, 'Nagyon jól együttműködtek.'),
(23, '2025-03-23 13:30:00', 8, 1, '00:44:05', 0, 'Ügyes, de lehetne gyorsabb.'),
(24, '2025-03-24 10:10:00', 9, 1, '00:46:50', 0, 'Kreatív megoldásokkal haladtak.'),
(25, '2025-03-25 14:20:00', 9, 1, '00:49:30', 0, 'Nagyon szoros volt.'),
(26, '2025-03-26 16:35:00', 9, 1, '00:47:10', 0, 'Remek teljesítmény.'),
(27, '2025-03-27 15:15:00', 1, 1, '00:38:45', 0, 'Leggyorsabb eddig!'),
(28, '2025-03-28 17:25:00', 2, 1, '00:41:30', 0, 'Kiemelkedő teljesítmény.'),
(29, '2025-03-29 19:00:00', 3, 1, '00:39:55', 0, 'Majdnem rekord!'),
(30, '2025-03-30 12:50:00', 4, 1, '00:42:40', 0, 'Nagyon ügyesek voltak.'),
(31, '2025-03-31 14:10:00', 5, 1, '00:45:20', 0, 'Izgalmas végjáték.'),
(32, '2025-04-01 15:30:00', 6, 1, '00:50:00', 0, 'A logikai feladatokat imádták.'),
(33, '2025-04-02 16:20:00', 7, 1, '00:43:30', 0, 'Jó csapatmunka, de lehetne gyorsabb.'),
(34, '2025-04-03 17:40:00', 8, 1, '00:41:15', 0, 'Meglepően gyors kijutás!');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `roles`
--

CREATE TABLE `roles` (
  `roleId` int(11) NOT NULL,
  `roleName` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `roles`
--

INSERT INTO `roles` (`roleId`, `roleName`) VALUES
(1, 'inactive'),
(2, 'active'),
(3, 'collegue'),
(4, 'admin');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `rooms`
--

CREATE TABLE `rooms` (
  `roomId` int(11) NOT NULL,
  `roomName` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `rooms`
--

INSERT INTO `rooms` (`roomId`, `roomName`) VALUES
(1, 'Menekülés az iskolából'),
(2, 'A pedellus bosszúja'),
(3, 'A tanári titkai'),
(4, 'A takarítónő visszanéz'),
(5, 'Szabadulás Kódja'),
(6, 'Időcsapda'),
(7, 'KódX Szoba'),
(8, 'Kalandok Kamrája'),
(9, 'Titkok Labirintusa');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `teams`
--

CREATE TABLE `teams` (
  `teamId` int(11) NOT NULL,
  `teamName` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `teams`
--

INSERT INTO `teams` (`teamId`, `teamName`) VALUES
(1, 'AdminTeam');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `users`
--

CREATE TABLE `users` (
  `userId` int(11) NOT NULL,
  `realName` varchar(255) NOT NULL,
  `nickName` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `phone` varchar(12) NOT NULL,
  `teamId` int(11) DEFAULT NULL,
  `roleId` int(11) DEFAULT NULL,
  `SALT` varchar(64) NOT NULL,
  `HASH` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `users`
--

INSERT INTO `users` (`userId`, `realName`, `nickName`, `email`, `phone`, `teamId`, `roleId`, `SALT`, `HASH`) VALUES
(1, 'Barany-Kiss Agnes', 'BKA', 'sindzse88@gmail.com', '+36706044782', 1, 4, '', ''),
(2, 'Juhasz Zsuzsanna', 'Zsu', 'lengyel.zsuzsanna@gmail.com', '+36701234567', 1, 4, '', ''),
(3, 'Meszaros Tibor', 'Tibi', 'meszarostibor74@outlook.hu', '+36701234567', 1, 4, '', ''),
(4, 'Kis Pista', 'Pisti', 'pisti@example.hu', '+36701234567', NULL, NULL, '', '');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `booking`
--
ALTER TABLE `booking`
  ADD PRIMARY KEY (`bookingId`),
  ADD KEY `roomId` (`roomId`,`teamId`),
  ADD KEY `teamId` (`teamId`);

--
-- A tábla indexei `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`roleId`);

--
-- A tábla indexei `rooms`
--
ALTER TABLE `rooms`
  ADD PRIMARY KEY (`roomId`);

--
-- A tábla indexei `teams`
--
ALTER TABLE `teams`
  ADD PRIMARY KEY (`teamId`);

--
-- A tábla indexei `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`userId`),
  ADD KEY `teamID` (`teamId`,`roleId`),
  ADD KEY `roleID` (`roleId`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `booking`
--
ALTER TABLE `booking`
  MODIFY `bookingId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;

--
-- AUTO_INCREMENT a táblához `roles`
--
ALTER TABLE `roles`
  MODIFY `roleId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT a táblához `rooms`
--
ALTER TABLE `rooms`
  MODIFY `roomId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT a táblához `teams`
--
ALTER TABLE `teams`
  MODIFY `teamId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT a táblához `users`
--
ALTER TABLE `users`
  MODIFY `userId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `booking`
--
ALTER TABLE `booking`
  ADD CONSTRAINT `booking_ibfk_1` FOREIGN KEY (`roomId`) REFERENCES `rooms` (`roomId`),
  ADD CONSTRAINT `booking_ibfk_2` FOREIGN KEY (`teamId`) REFERENCES `teams` (`teamId`);

--
-- Megkötések a táblához `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`roleID`) REFERENCES `roles` (`roleId`),
  ADD CONSTRAINT `users_ibfk_2` FOREIGN KEY (`teamID`) REFERENCES `teams` (`teamId`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
