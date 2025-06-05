DROP DATABASE IF EXISTS LibraryDB;

CREATE DATABASE LibraryDB
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE LibraryDB;

CREATE TABLE Book (
  ISBN VARCHAR(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  Name VARCHAR(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  Author VARCHAR(255) COLLATE utf8mb4_unicode_ci,
  Introduction TEXT COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (ISBN)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE User (
  UserId INT NOT NULL AUTO_INCREMENT,
  PhoneNumber VARCHAR(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  PasswordHash VARCHAR(256) COLLATE utf8mb4_unicode_ci NOT NULL,
  Salt VARCHAR(64) COLLATE utf8mb4_unicode_ci NOT NULL,
  UserName VARCHAR(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  RegistrationTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  LastLoginTime DATETIME DEFAULT NULL,
  PRIMARY KEY (UserId),
  UNIQUE KEY PhoneNumber (PhoneNumber)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE Inventory (
  InventoryId INT NOT NULL AUTO_INCREMENT,
  ISBN VARCHAR(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
  StoreTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  Status ENUM('Available','Borrowed','Restocking','Lost','Damaged','Discarded') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'Available',
  PRIMARY KEY (InventoryId),
  KEY ISBN (ISBN),
  CONSTRAINT inventory_ibfk_1 FOREIGN KEY (ISBN) REFERENCES Book (ISBN)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE BorrowingRecord (
  RecordId INT NOT NULL AUTO_INCREMENT,
  UserId INT,
  InventoryId INT,
  BorrowingTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  ReturnTime DATETIME DEFAULT NULL,
  PRIMARY KEY (RecordId),
  KEY UserId (UserId),
  KEY InventoryId (InventoryId),
  CONSTRAINT borrowingrecord_ibfk_1 FOREIGN KEY (UserId) REFERENCES User (UserId),
  CONSTRAINT borrowingrecord_ibfk_2 FOREIGN KEY (InventoryId) REFERENCES Inventory (InventoryId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

DELIMITER //
CREATE PROCEDURE sp_RegisterUser(
  IN in_phone VARCHAR(20),
  IN in_password VARCHAR(256),
  IN in_salt VARCHAR(64),
  IN in_username VARCHAR(100)
)
BEGIN
  IF EXISTS (SELECT 1 FROM User WHERE PhoneNumber = in_phone) THEN
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = '手機號碼已註冊';
  ELSE
    INSERT INTO User (PhoneNumber, PasswordHash, Salt, UserName, RegistrationTime)
    VALUES (in_phone, in_password, in_salt, in_username, NOW());
  END IF;
END;
//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE sp_GetUserAuthInfo(
  IN in_phone VARCHAR(20)
)
BEGIN
  SELECT UserId, UserName, PasswordHash, Salt
  FROM User
  WHERE PhoneNumber = in_phone;
END;
//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE sp_BorrowBook(
  IN in_userId INT,
  IN in_inventoryId INT
)
BEGIN
  START TRANSACTION;
  IF EXISTS (
    SELECT 1 FROM Inventory 
    WHERE InventoryId = in_inventoryId AND Status = 'Available'
  ) THEN
    UPDATE Inventory
    SET Status = 'Borrowed'
    WHERE InventoryId = in_inventoryId;

    INSERT INTO BorrowingRecord(UserId, InventoryId, BorrowingTime)
    VALUES (in_userId, in_inventoryId, NOW());

    COMMIT;
  ELSE
    ROLLBACK;
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = '此書已被借出或不存在';
  END IF;
END;
//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE sp_ReturnBook(
  IN in_userId INT,
  IN in_inventoryId INT
)
BEGIN
  START TRANSACTION;
  IF EXISTS (
    SELECT 1 FROM BorrowingRecord 
    WHERE UserId = in_userId AND InventoryId = in_inventoryId AND ReturnTime IS NULL
  ) THEN
    UPDATE Inventory
    SET Status = 'Available'
    WHERE InventoryId = in_inventoryId;

    UPDATE BorrowingRecord
    SET ReturnTime = NOW()
    WHERE UserId = in_userId AND InventoryId = in_inventoryId AND ReturnTime IS NULL;

    COMMIT;
  ELSE
    ROLLBACK;
    SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = '查無可還書籍';
  END IF;
END;
//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE sp_GetAvailableBooks()
BEGIN
  SELECT i.InventoryId, i.ISBN, b.Name AS BookName, b.Author, i.Status
  FROM Inventory i
  JOIN Book b ON i.ISBN = b.ISBN
  WHERE i.Status = 'Available';
END;
//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE sp_GetBorrowedBooksByUser(
  IN in_userId INT
)
BEGIN
  SELECT i.InventoryId, i.ISBN, b.Name AS BookName, b.Author, i.Status
  FROM BorrowingRecord br
  JOIN Inventory i ON br.InventoryId = i.InventoryId
  JOIN Book b ON i.ISBN = b.ISBN
  WHERE br.UserId = in_userId AND br.ReturnTime IS NULL;
END;
//
DELIMITER ;

DELIMITER $$

CREATE PROCEDURE sp_UpdateLastLoginTime(IN in_userId INT)
BEGIN
    UPDATE User
    SET LastLoginTime = NOW()
    WHERE UserId = in_userId;
END$$

DELIMITER ;