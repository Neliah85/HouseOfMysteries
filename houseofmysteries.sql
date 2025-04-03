-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Ápr 03. 14:37
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
(1, '2025-03-20 10:30:00', 1, 1, '00:52:30', 0, NULL),
(2, '2025-03-24 13:00:00', 2, 1, '01:05:45', 0, 'Karbantartás'),
(29, '2025-03-24 15:00:00', 1, 5, '00:58:15', 0, 'Karbantartás'),
(38, '2025-03-27 12:00:00', 1, 2, '00:55:00', 0, 'Karbantartás'),
(40, '2025-03-27 16:30:00', 1, 3, '01:01:20', 0, 'online foglalás'),
(41, '2025-03-26 13:30:00', 2, 1, '00:59:59', 0, 'online foglalás'),
(42, '2025-03-28 18:00:00', 2, 1, '00:51:00', 0, 'online foglalás'),
(43, '2025-03-25 18:00:00', 3, 2, '01:08:30', 0, 'online foglalás'),
(44, '2025-03-25 16:30:00', 3, 3, '00:54:00', 0, 'online foglalás'),
(45, '2025-03-25 15:00:00', 3, 3, '01:03:00', 0, 'online foglalás'),
(46, '2025-03-25 13:30:00', 3, 4, '00:57:40', 0, 'online foglalás'),
(47, '2025-03-25 10:30:00', 3, 4, '01:00:00', 0, 'online foglalás'),
(48, '2025-04-02 14:00:00', 4, 1, NULL, 0, 'Új foglalás'),
(49, '2025-04-05 16:30:00', 5, 2, NULL, 0, 'Új foglalás'),
(50, '2025-04-10 12:00:00', 6, 3, NULL, 0, 'Új foglalás'),
(51, '2025-04-15 15:00:00', 7, 4, NULL, 0, 'Új foglalás'),
(52, '2025-04-20 18:30:00', 8, 5, NULL, 0, 'Új foglalás'),
(53, '2025-04-25 17:00:00', 9, 1, NULL, 0, 'Új foglalás'),
(54, '2025-05-01 13:00:00', 1, 2, NULL, 0, 'Új foglalás'),
(55, '2025-05-05 16:00:00', 2, 3, NULL, 0, 'Új foglalás'),
(56, '2025-05-10 11:00:00', 3, 4, '00:04:00', 0, 'Új foglalás'),
(57, '2025-03-31 10:30:00', 1, 5, '00:56:20', 0, 'online foglalás'),
(65, '2025-04-08 18:00:00', 1, 1, NULL, 1, 'Karbantartás'),
(66, '2025-04-08 18:00:00', 2, 1, NULL, 0, 'Karbantartás'),
(67, '2025-04-10 09:00:00', 1, 9, NULL, 0, NULL),
(68, '2025-04-15 12:00:00', 2, 16, NULL, 0, NULL),
(69, '2025-04-22 15:00:00', 3, 13, NULL, 0, NULL),
(70, '2025-04-28 18:00:00', 4, 8, NULL, 0, NULL),
(71, '2025-05-03 10:30:00', 5, 4, NULL, 0, NULL),
(72, '2025-05-09 13:30:00', 6, 11, NULL, 0, NULL),
(73, '2025-05-16 16:30:00', 7, 2, NULL, 0, NULL),
(74, '2025-05-23 09:00:00', 8, 12, NULL, 0, NULL),
(75, '2025-05-30 12:00:00', 9, 8, NULL, 0, NULL),
(76, '2025-06-01 15:00:00', 1, 9, NULL, 0, NULL),
(77, '2025-06-01 18:00:00', 2, 4, NULL, 0, NULL),
(78, '2025-06-02 09:00:00', 3, 16, NULL, 0, NULL),
(79, '2025-06-02 12:00:00', 4, 16, NULL, 0, NULL),
(80, '2025-06-03 15:00:00', 5, 6, NULL, 0, NULL),
(81, '2025-06-03 18:00:00', 6, 13, NULL, 0, NULL),
(82, '2025-06-04 09:00:00', 7, 8, NULL, 0, NULL),
(83, '2025-06-04 12:00:00', 8, 16, NULL, 0, NULL),
(84, '2025-06-05 15:00:00', 9, 9, NULL, 0, NULL),
(85, '2025-06-05 18:00:00', 1, 10, NULL, 0, NULL),
(86, '2025-06-06 09:00:00', 2, 3, NULL, 0, NULL),
(87, '2025-06-06 12:00:00', 3, 3, NULL, 0, NULL),
(88, '2025-06-07 15:00:00', 4, 12, NULL, 0, NULL),
(89, '2025-06-07 18:00:00', 5, 11, NULL, 0, NULL),
(90, '2025-06-08 09:00:00', 6, 7, NULL, 0, NULL),
(91, '2025-06-08 12:00:00', 7, 2, NULL, 0, NULL),
(92, '2025-06-09 15:00:00', 8, 8, NULL, 0, NULL);

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
(1, 'AdminTeam'),
(2, 'Kiskacsa'),
(3, 'Macskák'),
(4, 'Macskák2'),
(5, 'Bélák'),
(6, 'Mystery Masters'),
(7, 'Escape Squad'),
(8, 'Code Breakers'),
(9, 'Puzzle Solvers'),
(10, 'The Key Hunters'),
(11, 'Mystery Masters'),
(12, 'Escape Squad'),
(13, 'Code Breakers'),
(14, 'Puzzle Soilders'),
(15, 'The Key Hunters'),
(16, 'Cirmosék');

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
(1, 'Barany-Kiss Agnes', 'BKA', 'sindzse88@gmail.com', '+36706044782', 1, 2, '', ''),
(3, 'Meszaros Tibor', 'Tibi', 'meszarostibor74@outlook.hu', '+36701234567', 1, 4, '', ''),
(4, 'Kis Pista', 'Pistike', 'lezsu@freemail.hu', '+36701234567', 2, 2, '', ''),
(18, 'Juhász Zsuzsanna Jolán', 'neliah69', 'cassandra.waldegrave@gmail.com', '+36703642200', 16, 4, '5d5a7920-79ef-4e28-962a-d3728166156f', '50a7efe2a7f15494af5b1c3e79589cafe0d60e6bdbfc1d38c3ba9afccad66d93'),
(36, 'Eva Molnar', 'EvaM', 'teszt1@teszt.hu', '+36701234567', 2, 2, '', ''),
(37, 'Andras Balogh', 'AndrasB', 'teszt1@teszt.hu', '+36701234567', 2, 2, '', ''),
(38, 'Gabor Farkas', 'GaborF', 'teszt1@teszt.hu', '+36701234567', 2, 2, '', ''),
(39, 'Janos Nemeth', 'JanosN', 'teszt1@teszt.hu', '+36701234567', 3, 2, '', ''),
(40, 'Beata Kiss', 'BeataK', 'teszt1@teszt.hu', '+36701234567', 3, 2, '', ''),
(41, 'Norbert Simon', 'NorbertS', 'teszt1@teszt.hu', '+36701234567', 3, 2, '', ''),
(42, 'Szilvia Orban', 'SzilviaO', 'teszt1@teszt.hu', '+36701234567', 3, 2, '', ''),
(43, 'Richard Biro', 'RichardB', 'teszt1@teszt.hu', '+36701234567', 3, 2, '', ''),
(44, 'Agnes Kovacs', 'AgnesK', 'teszt1@teszt.hu', '+36701234567', 4, 2, '', ''),
(45, 'Laszlo Fekete', 'LaszloF', 'teszt1@teszt.hu', '+36701234567', 4, 2, '', ''),
(46, 'Tamas Varga', 'TamasV', 'teszt1@teszt.hu', '+36701234567', 4, 2, '', ''),
(47, 'Monika Toth', 'MonikaT', 'teszt1@teszt.hu', '+36701234567', 4, 2, '', ''),
(48, 'Csaba Horvath', 'CsabaH', 'teszt1@teszt.hu', '+36701234567', 4, 2, '', ''),
(49, 'Edit Molnar', 'EditM', 'teszt1@teszt.hu', '+36701234567', 5, 2, '', ''),
(50, 'Ferenc Balogh', 'FerencB', 'teszt1@teszt.hu', '+36701234567', 5, 2, '', ''),
(51, 'Judit Farkas', 'JuditF', 'teszt1@teszt.hu', '+36701234567', 5, 2, '', ''),
(55, 'BKA', 'BaKissa', 'szoficica2017@gmail.com', '+36701234567', NULL, 2, 'f4d6d01a-f6ad-44d7-aa5c-a7909d7495c0', '619d84a04c226d3669c4f4b06da18f511f027385e397d2805d0562858da3eb89'),
(56, 'Juhász Tamás', 'Tomika', 'off20211115@gmail.com', '+36701258963', NULL, 2, '59e3f501-b8ab-48a6-9214-588fe2e2e8df', '73f74d3667e29b5c209611acfcad2e7e092dff41799e1843d585ae84161a9c79'),
(59, 'LeZsu', 'len', 'lengyel.zsuzsanna@gmail.com', '+36703642200', NULL, 1, '51e01deb-e6cc-407f-8d47-f8fe76a8d8cb', 'c5c69e1c3654f77565b3b938282bb123419b59db1bc9119eaa1914cb601c42cb');

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
  MODIFY `bookingId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=130;

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
  MODIFY `teamId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT a táblához `users`
--
ALTER TABLE `users`
  MODIFY `userId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=60;

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
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`roleId`) REFERENCES `roles` (`roleId`),
  ADD CONSTRAINT `users_ibfk_2` FOREIGN KEY (`teamId`) REFERENCES `teams` (`teamId`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
