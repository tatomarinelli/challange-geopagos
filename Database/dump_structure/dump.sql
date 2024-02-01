--
-- PostgreSQL database dump
--

-- Dumped from database version 15.5 (Debian 15.5-1.pgdg120+1)
-- Dumped by pg_dump version 15.3

-- Started on 2024-02-01 14:12:41

\connect geopagos_db

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 6 (class 2615 OID 24581)
-- Name: gp_schema; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA gp_schema;


ALTER SCHEMA gp_schema OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 216 (class 1259 OID 24582)
-- Name: operation_action; Type: TABLE; Schema: gp_schema; Owner: postgres
--

CREATE TABLE gp_schema.operation_action (
    action character varying NOT NULL,
    description character varying
);


ALTER TABLE gp_schema.operation_action OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 24587)
-- Name: operation_log; Type: TABLE; Schema: gp_schema; Owner: postgres
--

CREATE TABLE gp_schema.operation_log (
    operation_id integer NOT NULL,
    client_id character varying,
    client_type character varying,
    card_holder_name character varying,
    card_type character varying,
    card_number character varying,
    card_cvc character varying,
    card_expiration_date character varying,
    transaction_amount numeric,
    transaction_date date,
    operation_status character varying,
    last_modification date,
    operation_action character varying
);
ALTER TABLE geopagos_db.gp_schema.operation_log  
    ALTER COLUMN operation_id ADD GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 );


ALTER TABLE gp_schema.operation_log OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 24592)
-- Name: operation_pending; Type: TABLE; Schema: gp_schema; Owner: postgres
--

CREATE TABLE gp_schema.operation_pending (
    id integer NOT NULL,
    operation_id integer NOT NULL,
    expires_at date NOT NULL
);
ALTER TABLE geopagos_db.gp_schema.operation_pending  
    ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 );

ALTER TABLE gp_schema.operation_pending OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 24595)
-- Name: operation_status; Type: TABLE; Schema: gp_schema; Owner: postgres
--

CREATE TABLE gp_schema.operation_status (
    status character varying NOT NULL,
    description character varying
);


ALTER TABLE gp_schema.operation_status OWNER TO postgres;

--
-- TOC entry 3364 (class 0 OID 24582)
-- Dependencies: 216
-- Data for Name: operation_action; Type: TABLE DATA; Schema: gp_schema; Owner: postgres
--

INSERT INTO gp_schema.operation_action VALUES ('PAY', 'Payment');
INSERT INTO gp_schema.operation_action VALUES ('REV', 'Reverse');
INSERT INTO gp_schema.operation_action VALUES ('RET', 'Return');


--
-- TOC entry 3365 (class 0 OID 24587)
-- Dependencies: 217
-- Data for Name: operation_log; Type: TABLE DATA; Schema: gp_schema; Owner: postgres
--



--
-- TOC entry 3366 (class 0 OID 24592)
-- Dependencies: 218
-- Data for Name: operation_pending; Type: TABLE DATA; Schema: gp_schema; Owner: postgres
--



--
-- TOC entry 3367 (class 0 OID 24595)
-- Dependencies: 219
-- Data for Name: operation_status; Type: TABLE DATA; Schema: gp_schema; Owner: postgres
--

INSERT INTO gp_schema.operation_status VALUES ('AUT', 'Authorized');
INSERT INTO gp_schema.operation_status VALUES ('PEN', 'Pending');
INSERT INTO gp_schema.operation_status VALUES ('DEN', 'Denied');


--
-- TOC entry 3215 (class 2606 OID 24601)
-- Name: operation_log operation_log_pk; Type: CONSTRAINT; Schema: gp_schema; Owner: postgres
--

ALTER TABLE ONLY gp_schema.operation_log
    ADD CONSTRAINT operation_log_pk PRIMARY KEY (operation_id);


--
-- TOC entry 3217 (class 2606 OID 24603)
-- Name: operation_pending operation_pending_pk; Type: CONSTRAINT; Schema: gp_schema; Owner: postgres
--

ALTER TABLE ONLY gp_schema.operation_pending
    ADD CONSTRAINT operation_pending_pk PRIMARY KEY (id);


--
-- TOC entry 3219 (class 2606 OID 24605)
-- Name: operation_status operation_status_pk; Type: CONSTRAINT; Schema: gp_schema; Owner: postgres
--

ALTER TABLE ONLY gp_schema.operation_status
    ADD CONSTRAINT operation_status_pk PRIMARY KEY (status);


--
-- TOC entry 3220 (class 2606 OID 24606)
-- Name: operation_log operation_log_operation_status_fk; Type: FK CONSTRAINT; Schema: gp_schema; Owner: postgres
--

ALTER TABLE ONLY gp_schema.operation_log
    ADD CONSTRAINT operation_log_operation_status_fk FOREIGN KEY (operation_status) REFERENCES gp_schema.operation_status(status);


--
-- TOC entry 3221 (class 2606 OID 24611)
-- Name: operation_pending operation_pending_operation_log_fk; Type: FK CONSTRAINT; Schema: gp_schema; Owner: postgres
--

ALTER TABLE ONLY gp_schema.operation_pending
    ADD CONSTRAINT operation_pending_operation_log_fk FOREIGN KEY (operation_id) REFERENCES gp_schema.operation_log(operation_id);


-- Completed on 2024-02-01 14:12:41

--
-- PostgreSQL database dump complete
--

