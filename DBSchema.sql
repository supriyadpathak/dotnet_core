IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[books]') AND type in (N'U'))
Drop table books
BEGIN
CREATE TABLE [dbo].[books](
	BookId INT IDENTITY PRIMARY KEY,
    Title varchar(750),
    Author  varchar(500),
    ISBN varchar(250),
	Genre varchar(50),
	IsOnRent INT DEFAULT 0
)
END
select * from books
IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[rentbooks]') AND type in (N'U'))
Drop table rentbooks
BEGIN
CREATE TABLE [dbo].[rentbooks](
	BookId INT,
    CustomerName varchar(750),
    CustomerContactNumber  varchar(500),
    CustomerAdress varchar(250),
	RentDate datetime,
	RentPeriodInweeks INT,
)
END
select * from rentbooks
insert into books(Title,Author,ISBN,Genre)
values('The Great Gatsby','F. Scott Fitzgerald','9780743273565','Classics')

insert into books(Title,Author,ISBN,Genre)
values('To Kill a Mockingbird','Harper Lee','9780060935467','Classics')

insert into books(Title,Author,ISBN,Genre) values
('1984','George Orwell','9780451524935','Dystopian')

insert into books(Title,Author,ISBN,Genre) values
('Pride and Prejudice','Jane Austen','9780141199078','Romance')

insert into books(Title,Author,ISBN,Genre) values
('The Catcher in the Rye','J.D. Salinger','9780316769488','Classics')

insert into books(Title,Author,ISBN,Genre) values
('The Hobbit','J.R.R. Tolkien','9780547928227','Fantasy')

insert into books(Title,Author,ISBN,Genre) values
('Fahrenheit 451','Ray Bradbury','9781451673319','Science Fiction')

insert into books(Title,Author,ISBN,Genre) values
('The Book Thief','Markus Zusak','9780375842207','Historical Fiction')

insert into books(Title,Author,ISBN,Genre) values
('Moby-Dick','Herman Melville','9781503280786','Classics')

insert into books(Title,Author,ISBN,Genre) values
('War and Peace','Leo Tolstoy','9781400079988','Historical Fiction')