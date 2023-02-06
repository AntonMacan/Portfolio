/*
This script is used to generate database locally.
*/

//getting connection to database
const { pool } = require('../db');



const sql_create_tables_in_db = ` BEGIN;
CREATE TABLE IF NOT EXISTS public."Komentar"
(
    "KorisnikID" integer NOT NULL,
    "TrgovinaID" integer NOT NULL,
    "OpisKomentara" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Komentar_pkey" PRIMARY KEY ("KorisnikID", "TrgovinaID")
);

CREATE TABLE IF NOT EXISTS public."Korisnik"
(
    "ID" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "Ime" text COLLATE pg_catalog."default" NOT NULL,
    "Prezime" text COLLATE pg_catalog."default" NOT NULL,
    "Email" text COLLATE pg_catalog."default" NOT NULL,
    "Nadimak" text COLLATE pg_catalog."default" NOT NULL,
    "Lozinka" text COLLATE pg_catalog."default" NOT NULL,
    "RazinaPristupa" smallint NOT NULL DEFAULT 0,
    "ZabranjenPristup" boolean NOT NULL DEFAULT false,
    CONSTRAINT "Korisnik_pkey" PRIMARY KEY ("ID")
);

CREATE TABLE IF NOT EXISTS public."Obavijest"
(
    "ID" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "DatumVrijeme" timestamp without time zone NOT NULL,
    "Opis" text COLLATE pg_catalog."default" NOT NULL,
    "Procitano" boolean NOT NULL DEFAULT false,
    CONSTRAINT "Obavijest_pkey" PRIMARY KEY ("ID")
);

CREATE TABLE IF NOT EXISTS public."Oznake"
(
    "Barkod" text COLLATE pg_catalog."default" NOT NULL,
    "KorisnikID" integer NOT NULL,
    "Oznaka" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Oznake_pkey" PRIMARY KEY ("Barkod", "KorisnikID")
);

CREATE TABLE IF NOT EXISTS public."Pretinac"
(
    "KorisnikID" integer NOT NULL,
    "ObavijestID" integer NOT NULL,
    CONSTRAINT "Pretinac_pkey" PRIMARY KEY ("KorisnikID", "ObavijestID")
);

CREATE TABLE IF NOT EXISTS public."Privatnost"
(
    "KorisnikID" integer NOT NULL,
    "Ime" boolean NOT NULL DEFAULT false,
    "Prezime" boolean NOT NULL DEFAULT false,
    "Email" boolean NOT NULL DEFAULT false,
    "Nadimak" boolean NOT NULL DEFAULT false,
    CONSTRAINT "Privatnost_pkey" PRIMARY KEY ("KorisnikID")
);

CREATE TABLE IF NOT EXISTS public."Proizvod"
(
    "Barkod" text COLLATE pg_catalog."default" NOT NULL,
    "Naziv" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Proizvod_pkey" PRIMARY KEY ("Barkod")
);

CREATE TABLE IF NOT EXISTS public."ProizvodTrgovina"
(
    "Barkod" text COLLATE pg_catalog."default" NOT NULL,
    "TrgovinaID" integer NOT NULL,
    "Cijena" double precision NOT NULL,
    CONSTRAINT "ProizvodTrgovina_pkey" PRIMARY KEY ("Barkod", "TrgovinaID")
);

CREATE TABLE IF NOT EXISTS public."PromjenaCijeneKorisnik"
(
    "KorisnikID" integer NOT NULL,
    "Barkod" text COLLATE pg_catalog."default" NOT NULL,
    "TrgovinaID" integer NOT NULL,
    "DatumVrijeme" timestamp without time zone NOT NULL,
    "NovaCijena" double precision NOT NULL,
    "SlikaPath" text COLLATE pg_catalog."default" NOT NULL,
    "Status" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PromjenaCijeneKorisnik_pkey" PRIMARY KEY ("KorisnikID", "Barkod", "TrgovinaID", "DatumVrijeme")
);

CREATE TABLE IF NOT EXISTS public."PromjenaCijeneTrgovina"
(
    "Barkod" text COLLATE pg_catalog."default" NOT NULL,
    "TrgovinaID" integer NOT NULL,
    "DatumVrijeme" timestamp without time zone NOT NULL,
    "NovaCijena" double precision NOT NULL,
    CONSTRAINT "PromjenaCijeneTrgovina_pkey" PRIMARY KEY ("Barkod", "TrgovinaID", "DatumVrijeme")
);

CREATE TABLE IF NOT EXISTS public."Trgovina"
(
    "ID" integer NOT NULL ,
    "Naziv" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Trgovina_pkey" PRIMARY KEY ("ID")
);

CREATE TABLE IF NOT EXISTS public.session
(
    sid character varying COLLATE pg_catalog."default" NOT NULL,
    sess json NOT NULL,
    expire timestamp(6) without time zone NOT NULL,
    CONSTRAINT session_pkey PRIMARY KEY (sid)
);

ALTER TABLE IF EXISTS public."Komentar"
    ADD CONSTRAINT "Komentar_KorisnikID_fkey" FOREIGN KEY ("KorisnikID")
    REFERENCES public."Korisnik" ("ID") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Komentar"
    ADD CONSTRAINT "Komentar_TrgovinaID_fkey" FOREIGN KEY ("TrgovinaID")
    REFERENCES public."Trgovina" ("ID") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Oznake"
    ADD CONSTRAINT "Oznake_Barkod_fkey" FOREIGN KEY ("Barkod")
    REFERENCES public."Proizvod" ("Barkod") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Oznake"
    ADD CONSTRAINT "Oznake_KorisnikID_fkey" FOREIGN KEY ("KorisnikID")
    REFERENCES public."Korisnik" ("ID") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Pretinac"
    ADD CONSTRAINT "Pretinac_KorisnikID_fkey" FOREIGN KEY ("KorisnikID")
    REFERENCES public."Korisnik" ("ID") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Pretinac"
    ADD CONSTRAINT "Pretinac_ObavijestID_fkey" FOREIGN KEY ("ObavijestID")
    REFERENCES public."Obavijest" ("ID") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Privatnost"
    ADD CONSTRAINT "Privatnost_KorisnikID_fkey" FOREIGN KEY ("KorisnikID")
    REFERENCES public."Korisnik" ("ID") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;
CREATE INDEX IF NOT EXISTS "Privatnost_pkey"
    ON public."Privatnost"("KorisnikID");


ALTER TABLE IF EXISTS public."ProizvodTrgovina"
    ADD CONSTRAINT "ProizvodTrgovina_Barkod_fkey" FOREIGN KEY ("Barkod")
    REFERENCES public."Proizvod" ("Barkod") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."ProizvodTrgovina"
    ADD CONSTRAINT "ProizvodTrgovina_TrgovinaID_fkey" FOREIGN KEY ("TrgovinaID")
    REFERENCES public."Trgovina" ("ID") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."PromjenaCijeneKorisnik"
    ADD CONSTRAINT "PromjenaCijeneKorisnik_Barkod_fkey" FOREIGN KEY ("Barkod")
    REFERENCES public."Proizvod" ("Barkod") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS public."PromjenaCijeneKorisnik"
    ADD CONSTRAINT "PromjenaCijeneKorisnik_KorisnikID_fkey" FOREIGN KEY ("KorisnikID")
    REFERENCES public."Korisnik" ("ID") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS public."PromjenaCijeneKorisnik"
    ADD CONSTRAINT "PromjenaCijeneKorisnik_TrgovinaID_fkey" FOREIGN KEY ("TrgovinaID")
    REFERENCES public."Trgovina" ("ID") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION
    NOT VALID;


ALTER TABLE IF EXISTS public."PromjenaCijeneTrgovina"
    ADD CONSTRAINT "PromjenaCijeneTrgovina_Barkod_fkey" FOREIGN KEY ("Barkod")
    REFERENCES public."Proizvod" ("Barkod") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."PromjenaCijeneTrgovina"
    ADD CONSTRAINT "PromjenaCijeneTrgovina_TrgovinaID_fkey" FOREIGN KEY ("TrgovinaID")
    REFERENCES public."Trgovina" ("ID") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Trgovina"
    ADD CONSTRAINT "Trgovina_ID_fkey" FOREIGN KEY ("ID")
    REFERENCES public."Korisnik" ("ID") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;
CREATE INDEX IF NOT EXISTS "Trgovina_pkey"
    ON public."Trgovina"("ID");

    alter table "Oznake"
drop constraint "Oznake_pkey";

alter table "Oznake" 
add constraint "Oznake_pkey" primary key ("Barkod", "KorisnikID", "Oznaka");

alter table "PromjenaCijeneKorisnik"
add column "ID" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 )

COMMIT;
`;

(async () => {
    
	//executing sql query
    try {
		//creating domain tables
        await pool.query(sql_create_tables_in_db);
        console.log("Domain tables created.")

    } catch (err) {
        console.log("Error creating domain tables.");
        return console.log(err.message);
    }

    await pool.end();

})();