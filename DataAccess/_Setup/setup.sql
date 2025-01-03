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
    P_OPTIONS JSONB NOT NULL,
    P_ICON VARCHAR(50)
);

-- Alter User Related Tables to add constraints

ALTER TABLE USERS ADD CONSTRAINT FK_USER_UT FOREIGN KEY (U_TYPE) REFERENCES USER_TYPES(UT_ID);
ALTER TABLE USER_HABITS ADD CONSTRAINT FK_USER_HABITS_U FOREIGN KEY (U_ID) REFERENCES USERS(U_ID);
ALTER TABLE PRESETS ADD CONSTRAINT FK_PRESETS_U FOREIGN KEY (P_USER) REFERENCES USERS(U_ID);

-- Create Table Related Tables

CREATE TABLE TABLES (
    T_GUID VARCHAR(50) NOT NULL PRIMARY KEY,
    T_NAME VARCHAR(50) NOT NULL,
    T_MANUFACTURER VARCHAR(50) NOT NULL,
    T_API INT
);

CREATE TABLE SUBSCRIBERS (
    T_GUID VARCHAR(50) NOT NULL PRIMARY KEY,
    S_URI VARCHAR(2083) NOT NULL
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

CREATE TABLE HEALTH(
	H_ID SERIAL,
    H_DATE TIMESTAMP NOT NULL,
    U_ID INT NOT NULL,
    P_ID INT,
    H_POSITION INT NOT NULL,
    PRIMARY KEY (H_ID),
    FOREIGN KEY (U_ID) REFERENCES USERS(U_ID),
    FOREIGN KEY (P_ID) REFERENCES PRESETS(P_ID)
);

INSERT INTO APIS (A_NAME, A_CONFIG) VALUES 
('Linak Simulator API V2', '{"controller": "LinakSimulatorController"}'),
('Mock API', '{"controller": "MockTableController"}'),
('Linak API', '{"controller": "LinakTableController"}');

INSERT INTO TABLES (T_GUID, T_NAME, T_MANUFACTURER, T_API) VALUES 
('cd:fb:1a:53:fb:e6', 'DESK 4486', 'Linak A/S', 1);

INSERT INTO USER_TYPES (UT_NAME, UT_PERMISSIONS) VALUES 
('ADMIN', '["GODMODE"]'),
('EMPLOYEE', '["CanAccess_TablePage","CanAccess_SettingsPage","CanAccess_HealthPage"]'),
('CLEANER', '["CanAccess_SettingsPage","CanAccess_CleanerPage"]');

COMMIT;