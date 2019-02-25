<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
    <xsl:template match="testRun">
        <html>

        <head>
            <style>
                body {
                    font-family: Arial;
                }

                dl {
                    width: 395px;
                    font-size: 12px
                }

                dd {
                    margin-left: 120px
                }

                dd,
                dt {
                    padding-top: 5px;
                    padding-bottom: 5px;
                }

                dt {
                    float: left;
                    padding-right: 5px;
                    font-weight: bolder;
                }
                /* dd {padding-left: 5px;} Does not work */

                dt {
                    clear: left;
                }

                dt,
                dd {
                    min-height: 1.5em;
                }

                table,
                th,
                td {
                    border-style: solid;
                    border-collapse: collapse;
                    border-color: black;
                    border-width: 1px;
                    padding: 3px 7px 3px 7px;
                    font-size: 12px;
                }

                table {
                    width: 100%;
                }

                thead {
                    background: skyblue;
                }
            </style>
            <title>
                Test Run Results for Test Case&#160;
                <xsl:value-of select="info/testCaseId" />
            </title>
        </head>

        <body>
            <h1>
                Test Results for Test Case&#160;
                <xsl:value-of select="info/testCaseId" />
            </h1>
            <dl>
                <dt>Project Name:</dt>
                <dd>
                    <xsl:value-of select="info/teamProjectName" />
                </dd>
                <!--<dt>Test Plan:</dt>
                <dd>
                    <xsl:value-of select="info/testPlan" />
                </dd>
                <dt>Test Suite:</dt>
                <dd>
                    <xsl:value-of select="info/testSuite" />
                </dd>-->
                <dt>Test Result:</dt>
                <dd>
                    <xsl:variable name="testOutcome" select="info/testResult"/>
                    <xsl:choose>
                        <xsl:when test="$testOutcome = 'Pass'">
                            <xsl:attribute name="style">
                                <xsl:value-of select="'color:green;'" />
                            </xsl:attribute>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:attribute name="style">
                                <xsl:value-of select="'color:red;'" />
                            </xsl:attribute>
                        </xsl:otherwise>
                    </xsl:choose>
                    <xsl:value-of select="info/testResult" />
                </dd>
            </dl>
            <table>
                <thead>
                    <tr>
                        <th>Flow Identifier</th>
                        <th>Data Identifier</th>
                        <th>Action</th>
                        <th>Expected Result</th>
                        <th>Actual Result</th>
                        <th>Test Result</th>
                        <th>Value</th>
                    </tr>
                </thead>
                <tbody>
                    <xsl:for-each select="testFindings/testFinding">
                        <xsl:variable name="findingResult" select="testResult"/>
                        <tr>
                            <td>
                                <xsl:value-of select="flowIdentifier" />
                            </td>
                            <td>
                                <xsl:value-of select="dataIdentifier" />
                            </td>
                            <td>
                                <xsl:value-of select="action" />
                            </td>
                            <td>
                                <xsl:value-of select="expectedResult" />
                            </td>
                            <td>
                                <xsl:value-of select="actualResult" />
                            </td>
                            <td>
                                <xsl:choose>
                                    <xsl:when test="$findingResult = 'Pass'">
                                        <xsl:attribute name="style">
                                            <xsl:value-of select="'color:green;'" />
                                        </xsl:attribute>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <xsl:attribute name="style">
                                            <xsl:value-of select="'color:red;'" />
                                        </xsl:attribute>
                                    </xsl:otherwise>
                                </xsl:choose>
                                <xsl:value-of select="testResult" />
                            </td>
                            <td>
                                <xsl:choose>
                                    <xsl:when test="$findingResult = 'Pass'">
                                        <xsl:attribute name="style">
                                            <xsl:value-of select="'color:green;'" />
                                        </xsl:attribute>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <xsl:attribute name="style">
                                            <xsl:value-of select="'color:red;'" />
                                        </xsl:attribute>
                                    </xsl:otherwise>
                                </xsl:choose>
                                <xsl:value-of select="value" />
                            </td>
                        </tr>
                    </xsl:for-each>
                </tbody>
            </table>
        </body>

        </html>
    </xsl:template>
</xsl:stylesheet>