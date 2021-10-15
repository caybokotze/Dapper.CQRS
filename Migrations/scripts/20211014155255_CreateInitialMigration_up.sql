CREATE TABLE users
(
    id      int AUTO_INCREMENT PRIMARY KEY,
    name    varchar(50),
    surname varchar(50),
    email   varchar(50)
);
CREATE TABLE user_details
(
    id        int AUTO_INCREMENT PRIMARY KEY,
    user_id   INTEGER NOT NULL,
    id_number varchar(16),
    FOREIGN KEY (user_id) REFERENCES users (id)
);
CREATE TABLE user_type
(
    id      int AUTO_INCREMENT PRIMARY KEY,
    user_id INTEGER NOT NULL,
    name    varchar(50),
    FOREIGN KEY (user_id) REFERENCES users (id)
);