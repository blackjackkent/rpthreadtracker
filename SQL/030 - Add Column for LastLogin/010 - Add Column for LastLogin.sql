ALTER TABLE UserProfile
ADD LastLogin DateTime DEFAULT GetDate()

UPDATE UserProfile SET LastLogin = '1900-01-01 00:00:00.000'