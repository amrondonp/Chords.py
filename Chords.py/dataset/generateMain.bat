echo off
type nul > data.csv
echo c,c#,d,d#,e,f,f#,g,g#,a,a#,b,chord >> data.csv
for /f "tokens=*" %%x in (a.csv) do (echo %%x,a) >> data.csv
for /f "tokens=*" %%x in (am.csv) do (echo %%x,am) >> data.csv
for /f "tokens=*" %%x in (bm.csv) do (echo %%x,bm) >> data.csv
for /f "tokens=*" %%x in (c.csv) do (echo %%x,c) >> data.csv
for /f "tokens=*" %%x in (d.csv) do (echo %%x,d) >> data.csv
for /f "tokens=*" %%x in (dm.csv) do (echo %%x,dm) >> data.csv
for /f "tokens=*" %%x in (e.csv) do (echo %%x,e) >> data.csv
for /f "tokens=*" %%x in (em.csv) do (echo %%x,em) >> data.csv
for /f "tokens=*" %%x in (f.csv) do (echo %%x,f) >> data.csv
for /f "tokens=*" %%x in (g.csv) do (echo %%x,g) >> data.csv
