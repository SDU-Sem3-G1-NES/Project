BEGIN;

-- User Related Tables

CREATE TABLE USERS (
    U_ID SERIAL PRIMARY KEY,
    U_NAME VARCHAR(50) NOT NULL,
    U_MAIL VARCHAR(72) NOT NULL, -- 72 is the maximum input length for BCrypt
    U_TYPE INT NOT NULL
);

CREATE TABLE USER_CREDENTIALS (
    -- Bcrypt has an output of 60 bytes
    UMAIL_HASH BYTEA NOT NULL PRIMARY KEY,
    UPASS_HASH BYTEA NOT NULL
);

CREATE TABLE USER_TYPES (
    UT_ID SERIAL PRIMARY KEY,
    UT_NAME VARCHAR(50) NOT NULL,
    UT_PERMISSIONS JSONB NOT NULL -- binary json
);

CREATE TABLE USER_HABITS (
    H_ID SERIAL PRIMARY KEY,
    U_ID INT NOT NULL,
    H_EVENT JSONB NOT NULL
);

CREATE TABLE PRESETS (
    P_ID SERIAL PRIMARY KEY,
    P_NAME VARCHAR(50) NOT NULL,
    P_USER INT NOT NULL,
    P_HEIGHT INT NOT NULL,
    P_OPTIONS JSONB NOT NULL
);

ALTER TABLE PRESETS ADD COLUMN P_ICON VARCHAR(50);

-- Alter User Related Tables to add constraints

ALTER TABLE USERS ADD CONSTRAINT FK_USER_UT FOREIGN KEY (U_TYPE) REFERENCES USER_TYPES(UT_ID);
ALTER TABLE USER_HABITS ADD CONSTRAINT FK_USER_HABITS_U FOREIGN KEY (U_ID) REFERENCES USERS(U_ID);
ALTER TABLE PRESETS ADD CONSTRAINT FK_PRESETS_U FOREIGN KEY (P_USER) REFERENCES USERS(U_ID);

-- Create Table Related Tables

CREATE TABLE TABLES (
    T_GUID VARCHAR(50) NOT NULL PRIMARY KEY,
    T_NAME VARCHAR(50) NOT NULL,
    T_MANUFACTURER VARCHAR(50) NOT NULL,
    T_API INT NOT NULL
);

CREATE TABLE APIS (
    A_ID SERIAL PRIMARY KEY,
    A_NAME VARCHAR(50) NOT NULL,
    A_CONFIG JSONB NOT NULL
);

CREATE TABLE ROOMS (
    R_ID SERIAL PRIMARY KEY,
    R_NAME VARCHAR(50) NOT NULL,
    R_NUMBER VARCHAR(10),
    R_FLOOR INT
);

CREATE TABLE SCHEDULES (
    S_ID SERIAL PRIMARY KEY,
    S_NAME VARCHAR(50) NOT NULL,
    S_CONFIG JSONB NOT NULL,
    S_OWNER INT NOT NULL
);

-- Alter Table Related Tables to add constraints

ALTER TABLE TABLES ADD CONSTRAINT FK_TABLES_A FOREIGN KEY (T_API) REFERENCES APIS(A_ID);
ALTER TABLE SCHEDULES ADD CONSTRAINT FK_SCHEDULES_U FOREIGN KEY (S_OWNER) REFERENCES USERS(U_ID);

-- Create Relational Tables

CREATE TABLE USER_TABLES (
    U_ID INT NOT NULL,
    T_GUID VARCHAR(50) NOT NULL,
    PRIMARY KEY (U_ID, T_GUID),
    FOREIGN KEY (U_ID) REFERENCES USERS(U_ID),
    FOREIGN KEY (T_GUID) REFERENCES TABLES(T_GUID)
);

CREATE TABLE ROOM_TABLES (
    R_ID INT NOT NULL,
    T_GUID VARCHAR(50) NOT NULL,
    PRIMARY KEY (R_ID, T_GUID),
    FOREIGN KEY (R_ID) REFERENCES ROOMS(R_ID),
    FOREIGN KEY (T_GUID) REFERENCES TABLES(T_GUID)
);

CREATE TABLE SCHEDULE_TABLES (
    S_ID INT NOT NULL,
    T_GUID VARCHAR(50) NOT NULL,
    PRIMARY KEY (S_ID, T_GUID),
    FOREIGN KEY (S_ID) REFERENCES SCHEDULES(S_ID),
    FOREIGN KEY (T_GUID) REFERENCES TABLES(T_GUID)
);

-- Insert data into USER_TYPES
INSERT INTO USER_TYPES (UT_NAME, UT_PERMISSIONS) VALUES
('Admin', '{"can_create": true, "can_edit": true, "can_delete": true}'),
('User', '{"can_create": true, "can_edit": false, "can_delete": false}');

-- Insert data into USERS
INSERT INTO USERS (U_NAME, U_MAIL, U_TYPE) VALUES
('John Doe', 'john.doe@example.com', 1),
('Jane Smith', 'jane.smith@example.com', 2);

-- Insert data into USER_CREDENTIALS
INSERT INTO USER_CREDENTIALS (UMAIL_HASH, UPASS_HASH) VALUES
('\x1234567890abcdef', '\xabcdef1234567890'),
('\xabcdef1234567890', '\x1234567890abcdef');

-- Insert data into USER_HABITS
INSERT INTO USER_HABITS (U_ID, H_EVENT) VALUES
(1, '{"habit": "running", "frequency": "daily"}'),
(2, '{"habit": "reading", "frequency": "weekly"}');

-- Insert data into APIS
INSERT INTO APIS (A_NAME, A_CONFIG) VALUES
('API1', '{"config1": "value1"}'),
('API2', '{"config2": "value2"}');

-- Insert data into TABLES
INSERT INTO TABLES (T_GUID, T_NAME, T_MANUFACTURER, T_API) VALUES
('guid1', 'Table1', 'Manufacturer1', 1),
('guid2', 'Table2', 'Manufacturer2', 2);

-- Insert data into ROOMS
INSERT INTO ROOMS (R_NAME, R_NUMBER, R_FLOOR) VALUES
('Room1', '101', 1),
('Room2', '102', 2);

-- Insert data into SCHEDULES
INSERT INTO SCHEDULES (S_NAME, S_CONFIG, S_OWNER) VALUES
('Schedule1', '{"config1": "value1"}', 1),
('Schedule2', '{"config2": "value2"}', 2);

-- Insert data into USER_TABLES
INSERT INTO USER_TABLES (U_ID, T_GUID) VALUES
(1, 'guid1'),
(2, 'guid2');

-- Insert data into ROOM_TABLES
INSERT INTO ROOM_TABLES (R_ID, T_GUID) VALUES
(1, 'guid1'),
(2, 'guid2');

-- Insert data into SCHEDULE_TABLES
INSERT INTO SCHEDULE_TABLES (S_ID, T_GUID) VALUES
(1, 'guid1'),
(2, 'guid2');

COMMIT;