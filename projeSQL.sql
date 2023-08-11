/*
use YemekProje;

create table il(
	ilID int identity(1,1) not null,
	ilAdi varchar(64) not null,
	primary key(ilID));

create table ilce(
	ilceID int identity(1,1) not null,
	ilID int not null,
	ilceAdi varchar(64) not null,
	primary key(ilceID),
	foreign key(ilID) references il(ilID));
	
create table musteri(
	musteriID int identity(1,1) not null,
	ad varchar(32) not null,
	soyad varchar(32) not null,
	sifre varchar(16) not null,
	eposta varchar(64) not null,
	ilID int not null,
	ilceID int not null,
	telNo varchar(11),
	silindi bit default 0 not null,
	primary key(musteriID),
	foreign key(ilID) references il(ilID),
	foreign key(ilceID) references ilce(ilceID));

create table sirket(
	sirketID int identity(1,1) not null,
	sirketAdi varchar(64) not null,
	silindi bit default 0 not null,
	primary key(sirketID));

create table restoran(
	restoranID int identity(1,1) not null,
	restoranAdi varchar(64) not null,
	sirketID int not null,
	ilID int not null,
	ilceID int not null,
	silindi bit default 0 not null,
	primary key(restoranID),
	foreign key(ilID) references il(ilID),
	foreign key(ilceID) references ilce(ilceID),
	foreign key(sirketID) references sirket(sirketID));
	
create table personel(
	personelID int identity(1,1) not null,
	ad varchar(32) not null,
	soyad varchar(32) not null,
	eposta varchar(64) not null,
	sifre varchar(16) not null,
	silindi bit not null default 0,
	restoranID int not null,
	primary key(personelID),
	foreign key(restoranID) references restoran(restoranID));

create table siparisler(
	siparisID int identity(1,1) not null,
	musteriID int not null,
	personelID int not null,
	tarih datetime not null default getdate(),
	tamamlandi bit not null default 0,
	primary key(siparisID),
	foreign key(musteriID) references musteri(musteriID),
	foreign key(personelID) references personel(personelID));

create table kategoriler(
	kategoriID int identity(1,1) not null,
	kategoriAdi varchar(64) not null,
	primary key(kategoriID));
	
create table urunler(
	urunID int identity(1,1) not null,
	restoranID int not null,
	urunAdi varchar(64) not null,
	fiyat float not null,
	kategoriID int not null,
	silindi bit not null default 0,
	primary key(urunID),
	foreign key(restoranID) references restoran(restoranID),
	foreign key(kategoriID) references kategoriler(kategoriID));
	
create table siparisicerik(
	siparisID int not null,
	urunID int not null,
	adet int not null,
	primary key(siparisID, urunID),
	foreign key(siparisID) references siparisler(siparisID),
	foreign key(urunID) references urunler(urunID));

create table kampanya(
	kampanyaID int identity(1,1) not null,
	urunID int not null,
	kampanyaYuzdesi int not null,
	aktif bit default 0 not null,
	primary key(kampanyaID),
	foreign key(urunID) references urunler(urunID));

create table adminler(
	adminID int identity(1,1) not null,
	ad varchar(32) not null,
	soyad varchar(32) not null,
	eposta varchar(64) not null,
	sifre varchar(16) not null,
	primary key(adminID));
	
insert into il values('İstanbul');
insert into il values('İzmir');
insert into il values('Ankara');
insert into il values('Antalya');

insert into ilce values(1, 'Kadıköy');
insert into ilce values(1, 'Küçükçekmece');
insert into ilce values(2, 'Bornova');
insert into ilce values(2, 'Karşıyaka');
insert into ilce values(3, 'Çankaya');
insert into ilce values(3, 'Keçiören');
insert into ilce values(4, 'Manavgat');
insert into ilce values(4, 'Alanya');

alter table musteri add constraint unqmusteri unique (eposta);
alter table personel add constraint unqpersonel unique (eposta);
alter table adminler add constraint unqadminler unique (eposta);

insert into adminler values('Yunus', 'Emre', 'yunus@gmail.com', '1234');

insert into musteri values('Yunus', 'Emre', '1234', 'emre@gmail.com', 1, 2, '05555555555', 0);
insert into musteri values('Walter', 'White', '1234', 'walter@gmail.com', 4, 8, '05444444444', 0);
insert into musteri values('Jesse', 'Pinkman', '1234', 'jesse@gmail.com', 2, 3, '05333333333', 0);

insert into kategoriler values('Ana Yemekler');
insert into kategoriler values('Yan Ürünler');
insert into kategoriler values('Tatlılar');
insert into kategoriler values('İçecekler');

insert into sirket values('Burger King', 0);
insert into sirket values('Popeyes', 0);
insert into sirket values('Dominos Pizza', 0);
insert into sirket values('Starbucks Coffee', 0);

insert into restoran values('Burger King-Kadıköy', 1, 1, 1, 0); 
insert into restoran values('Burger King-Bornova', 1, 2, 3, 0); 
insert into restoran values('Burger King-Manavgat', 1, 4, 7, 0); 
insert into restoran values('Popeyes', 2, 1, 2, 0); 
insert into restoran values('Pizzacı', 3, 1, 1, 0); 
insert into restoran values('Star Kahve', 4, 3, 5, 0); 

create view restoranGetir as
select tmp2.*, sirketAdi from sirket inner join (select tmp.*, ilce.ilceAdi from ilce inner join 
(select r.*, il.ilAdi from restoran r inner join il on il.ilID = r.ilID) as tmp 
on ilce.ilceID = tmp.ilceID) as tmp2 on tmp2.sirketID = sirket.sirketID;

insert into urunler values(2, 'Double Burger', 69.90, 1, 0);
insert into urunler values(2, 'Triple Burger', 89.90, 1, 0);
insert into urunler values(2, 'Sandviç', 35.90, 2, 0);
insert into urunler values(2, 'Coca-Cola', 19.90, 4, 0);
insert into urunler values(2, 'Coca-Cola Zero', 19.90, 4, 0);

create view urunGetir as
select bir.kampanyaYuzdesi, temp.* from (select * from kampanya where aktif = 1) as bir right join (select u.*, k.kategoriAdi from urunler u inner join kategoriler k on k.kategoriID = u.kategoriID) as temp on temp.urunID = bir.urunID;

insert into personel values('Saul', 'Goodman', 'saul@gmail.com', '1234', 0, 2);


create view urunGetir2 as
select bir.kampanyaYuzdesi, bir.kampanyaID, temp.* from (select * from kampanya where aktif = 1) as bir right join (select u.*, k.kategoriAdi from urunler u inner join kategoriler k on k.kategoriID = u.kategoriID) as temp on temp.urunID = bir.urunID;


insert into personel values('Necmi', 'Necmioğlu', 'necmi@gmail.com', '1234', 0, 1);


select s.siparisID, s.musteriID, s.tarih, s.tamamlandi, u.* from siparisler s inner join urunler u on s.urunId = u.urunID;

select m.ad, m.soyad, tmp.* from musteri m inner join (select s.siparisID, s.musteriID, s.tarih, s.tamamlandi, u.* from siparisler s inner join urunler u on s.urunId = u.urunID) tmp
on tmp.musteriID = m.musteriID
*/
create view siparisGetir as 
select m.ad, m.soyad, tmp.* from musteri m inner join (select s.siparisID, s.musteriID, s.tarih, s.tamamlandi, u.* from siparisler s inner join urunler u on s.urunId = u.urunID) tmp
on tmp.musteriID = m.musteriID







