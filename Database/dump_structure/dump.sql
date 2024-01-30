--
-- PostgreSQL database dump
--

-- Dumped from database version 15.5 (Debian 15.5-1.pgdg120+1)
-- Dumped by pg_dump version 15.3

-- Started on 2024-01-30 00:36:11

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
-- TOC entry 6 (class 2615 OID 24576)
-- Name: gp_schema; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA gp_schema;
DROP SCHEMA public CASCADE;


ALTER SCHEMA gp_schema OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 215 (class 1259 OID 24577)
-- Name: newtable; Type: TABLE; Schema: gp_schema; Owner: postgres
--

CREATE TABLE gp_schema.newtable (
);


ALTER TABLE gp_schema.newtable OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 24580)
-- Name: newtable_1; Type: TABLE; Schema: gp_schema; Owner: postgres
--

CREATE TABLE gp_schema.newtable_1 (
);


ALTER TABLE gp_schema.newtable_1 OWNER TO postgres;

--
-- TOC entry 3345 (class 0 OID 24577)
-- Dependencies: 215
-- Data for Name: newtable; Type: TABLE DATA; Schema: gp_schema; Owner: postgres
--

COPY gp_schema.newtable  FROM stdin;
\.


--
-- TOC entry 3346 (class 0 OID 24580)
-- Dependencies: 216
-- Data for Name: newtable_1; Type: TABLE DATA; Schema: gp_schema; Owner: postgres
--

COPY gp_schema.newtable_1  FROM stdin;
\.


-- Completed on 2024-01-30 00:36:11

--
-- PostgreSQL database dump complete
--

