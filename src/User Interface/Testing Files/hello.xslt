<?xml version="1.0"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- Use the XLST arguments of "saygoodbye,,true" to activate the argument test. -->
	<xsl:param name="saygoodbye"/>

	<xsl:template match="/helloworld">
		<HTML>
			<HEAD>
				<TITLE>XSLT Processing Test</TITLE>
			</HEAD>
			<BODY>
				<H1>
					<xsl:value-of select="greeting"/>
				</H1>
				<xsl:apply-templates select="description"/>
				<H2>
					<xsl:if test="$saygoodbye='true'">
						<DIV>Goodbye!</DIV>
					</xsl:if>
				</H2>
			</BODY>
		</HTML>
	</xsl:template>
	<xsl:template match="description">
		<DIV>
			<xsl:value-of select="."/>
		</DIV>
	</xsl:template>
</xsl:stylesheet>