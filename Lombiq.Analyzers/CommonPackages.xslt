<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" encoding="utf-8" indent="yes"/>
    <xsl:strip-space elements="*"/>

    <xsl:template match="/">
        <xsl:comment>
            This file is generated on build. Please edit dependencies.xml instead and then build Lombiq.Analyzers 
            project twice. It has to be twice, because a transformation can only run in Targets, but Import can't be in 
            Targets so it will necessarily happen before the transformation can update this file.</xsl:comment>
        <Project>
            <ItemGroup>
                <xsl:for-each select="dependencies/dependency">
                    <PackageReference IncludeInPackage="true">
                        <xsl:attribute name="Include">
                            <xsl:value-of select="./@id"/>
                        </xsl:attribute>
                        
                        <xsl:attribute name="Version">
                            <xsl:value-of select="./@version"/>
                        </xsl:attribute>

                        <PrivateAssets>all</PrivateAssets>
                        <IncludeAssets>runtime; build; native; contentfiles; analyzers;</IncludeAssets>
                    </PackageReference>
                </xsl:for-each>
            </ItemGroup>
        </Project>
    </xsl:template>
</xsl:stylesheet> 