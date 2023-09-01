using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using Word2010 = DocumentFormat.OpenXml.Office2010.Word;

namespace naic
{
    /**
    \brief
        Provides a mechanism to read,
        write and manage content control
        elements in an OpenXML document.
    */
    public class ContentControlProcessor
    {
        /// Wordprocessing document that this
        /// processor will manage
        public WordprocessingDocument wordprocessingDocument;

        /**
        \brief
            Constructor.

        \param document
            WordprocessingDocument that this
            processor manages.
        */
        public ContentControlProcessor(WordprocessingDocument document)
        {
            // Assign wordprocessing document
            this.wordprocessingDocument = document;
        }

        /**
        \brief
            Get the content contained within the
            given SdtElement.

            This checks for content blocks, cells,
            runs, rows, etc. and returns
            the first that is not null.

        \param element
            Content control element to get content
            for

        \return
            Content contained within the given content
            control, or null if none is found
        */
        private OpenXmlElement GetControlContentBlock(SdtElement element)
        {
            // Retrieve any possible child content elements
            OpenXmlElement[] content = new OpenXmlElement[4];

            OpenXmlElement outputElement = null;

            content[0] = element.GetFirstChild<SdtContentBlock>();
            content[1] = element.GetFirstChild<SdtContentRun>();
            content[2] = element.GetFirstChild<SdtContentCell>();
            content[3] = element.GetFirstChild<SdtContentRow>();

            // Iterate through each possible source
            // of content until something is found
            // or until we've tried everything we can
            for (int i = 0; (outputElement == null && i < content.Count()); ++i)
            {
                // Assign content
                outputElement = content[i];
            }

            // Return the element
            return outputElement;
        }

        /**
        \brief
            Returns the first content control with the
            given tag.

        \param tag
            Tag

        \return
            First content control with the given tag,
            or null if none exists.
        */
        public SdtElement ContentControlAtTag(string tag)
        {
            // Refer to the main document portion of
            // the NAIC template
            Document naicDoc = this.wordprocessingDocument.MainDocumentPart.Document;

            foreach(SdtElement element in naicDoc.Descendants<SdtElement>().ToList())
            {
                // Create a variable for the element's
                // properties and tag
                SdtProperties elProperties = null;
                Tag elTag = null;

                // Get the element's properties
                elProperties = element.GetFirstChild<SdtProperties>();

                // Make sure they're not null
                if(elProperties == null)
                {
                    continue;
                }

                // Get the properties' tag
                elTag = elProperties.GetFirstChild<Tag>();

                // Make sure it's not null
                if(elTag == null)
                {
                    continue;
                }

                // Check for a match
                if(elTag.Val.Value == tag)
                {
                    // It's a match!
                    // Return this element
                    return element;
                }
            }

            return null;
        }

        /**
        \brief
            Return the tag of the given content
            control element.

            If the given element does not have a
            tag, null is returned.

        \param element
            Content control element to retrieve
            tag from

        \return
            Tag for content control element, or
            null if one does not exist
        */
        public string TagAtContentControl(SdtElement element)
        {
            // Declare variables for element properties
            // and tag
            SdtProperties properties = null;
            Tag tag = null;

            // Get element properties
            properties = element.GetFirstChild<SdtProperties>();

            // Make sure properties aren't null
            if(properties == null)
            {
                return null;
            }

            // Get tag from properties
            tag = properties.GetFirstChild<Tag>();

            // Check if tag actually exists
            if(tag == null)
            {
                // It doesn't. Return null.
                return null;
            }

            // The tag does exist. Return
            // its text.
            return tag.Val.Value;
        }

        /**
        \brief
            Flattens all tagged content controls
            within the document.
        */
        public void FlattenAllContent()
        {
            // Refer to the main document portion of
            // the NAIC template
            Document naicDoc = this.wordprocessingDocument.MainDocumentPart.Document;

            // Get each content control
            foreach(SdtElement element in naicDoc.Descendants<SdtElement>().ToList())
            {
                // Make sure element has properties
                if(element.SdtProperties == null)
                {
                    continue;
                }

                // Get tag
                Tag elementTag = element.SdtProperties.GetFirstChild<Tag>();

                // Make sure element has tag
                if(elementTag == null)
                {
                    continue;
                }
                // Has a tag. Flatten control.

                // Flatten control
                this.FlattenContent(element);
            };
        }
        
        /**
        \brief
            Flatten the content at the given content
            control element.

        \param element
            Content control element. Given element
            must be a content control element.
        */
        public void FlattenContent(SdtElement element)
        {
            // Assign the parent element
            OpenXmlElement parentElement = element.Parent;

            // Assign the content element
            OpenXmlElement contentElement;
            
            // Get content if it exists
            contentElement = this.GetControlContentBlock(element);

            // Check if content is still null
            if (contentElement == null)
            {
                // It is. That means there was
                // no content. Remove the content
                // control and return.
                element.Remove();

                return;
            }

            // Copy everything in the content element
            // into the parent of the content control
            foreach (OpenXmlElement contentChild in contentElement.ChildElements)
            {
                // Insert a copy of the contentChild element
                // after the original element.
                parentElement.InsertAfter(
                    (OpenXmlElement)contentChild.Clone(),
                    (OpenXmlElement)element);
            }

            // Remove the content control, leaving only
            // the content that was copied earlier.
            element.Remove();
        }

        /**
        \brief
            Removes each content control with the
            given tag, and inserts its content
            in its place.

        \param tag
            Tag of content control(s) to remove.
        */
        public void FlattenContentAtTag(string tag)
        {
            // Refer to the main document portion of
            // the NAIC template
            Document naicDoc = this.wordprocessingDocument.MainDocumentPart.Document;

            // Iterate through each content control (SDT)
            // in the document
            foreach(SdtElement element in naicDoc.Descendants<SdtElement>().ToList())
            {
                // Make sure element has properties
                if(element.SdtProperties == null)
                {
                    continue;
                }

                // Get tag
                Tag elementTag = element.SdtProperties.GetFirstChild<Tag>();

                // Make sure element has tag
                if(elementTag == null)
                {
                    continue;
                }

                // Check if content control tag matches tag
                // argument
                if (elementTag.Val.Value != tag)
                {
                    // It does not match.
                    // Continue looping.
                    continue;
                }
                // It matches.

                // Flatten the content
                this.FlattenContent(element);
            }
        }

        /**
        \brief
            Sets the check box state for the check
            cbox content control of the given tag

        \param tag
            Check box content control tag

        \param isChecked
            Whether or not check box should be checked
        */
        public void SetCheckBoxStateAtTag(string tag, bool isChecked)
        {
            // Refers to the main document portion of
            // the NAIC document
            Document naicDoc = this.wordprocessingDocument.MainDocumentPart.Document;

            // Iterate through each SDT element in
            // the document
            foreach (SdtElement element in naicDoc.Descendants<SdtElement>().ToList())
            {
                // Iterate through each tag in the check box content
                // control
                foreach (Tag checkTag in element.Descendants<Tag>().ToList())
                {
                    // Check for a match
                    if (checkTag.Val.Value != tag)
                    {
                        // No match, continue
                        // looping

                        continue;
                    }

                    // Tag matches.
                    // Get the check box control

                    // Create a list containing each
                    // check box control
                    List<Word2010.SdtContentCheckBox> checkBoxList;
                    checkBoxList = element.Descendants<Word2010.SdtContentCheckBox>().ToList();

                    // Iterate through list
                    foreach(Word2010.SdtContentCheckBox checkBox in checkBoxList)
                    {
                        // Determine which character to use
                        // for the checked/unchecked states
                        char checkedChar = '\u2610'; // "☐" character

                        if (isChecked == true)
                        {
                            checkedChar = '\u2611'; // "☑" Character
                        }

                        // Create a variable to store whether or not
                        // the check box should be checked.
                        // By default, it will not be.
                        Word2010.OnOffValues checkState = Word2010.OnOffValues.False;

                        // If the isChecked argument is true,
                        // then we should tick the check box
                        if(isChecked == true)
                        {
                            // The check state should be set to true
                            checkState = Word2010.OnOffValues.True;
                        }

                        // Update check box state
                        checkBox.Checked.Val = checkState;

                        // Now update content text to reflect
                        // the checked state
                        // Get content contained with content
                        // control
                        OpenXmlElement contentElement;

                        contentElement = this.GetControlContentBlock(element);

                        // Make sure there is indeed
                        // content.
                        if (contentElement == null)
                        {
                            // No content, so nothing
                            // to replace.
                            // Continue looping
                            continue;
                        }

                        // Iterate through each text element
                        // contained within the content control's
                        // content block/run/row/cell
                        foreach(Text text in contentElement.Descendants<Text>().ToList())
                        {
                            // Assign text to the correct
                            // checked character
                            text.Text = checkedChar.ToString();
                        }
                    }
                }
            }
        }

        /**
        \brief
            Sets the given content control's
            font size.

            Finds the content control using
            its tag.

        \param tag
            Tag to use to find content control

        \param size
            Font size
        */
        public void SetContentFontSizeAtTag(
            string tag,
            int size)
        {
            // Get element with tag
            SdtElement element = this.ContentControlAtTag(tag);

            // Make sure element exists.
            if(element == null)
            {
                // It doesn't.
                // Return.
                return;
            }

            // Set font size
            this.SetContentFontSize(element, size);
        }


        /**
        \brief
            Sets the given content control's
            font size.

        \param element
            Content control element

        \param size
            Font size
        */
        public void SetContentFontSize(
            SdtElement element,
            int size)
        {
            // Get content control content
            OpenXmlElement content = this.GetControlContentBlock(element);
            
            // Make sure content exists.
            if(content == null)
            {
                // It does not.
                // Return
                return;
            }

            // Iterate through each run property
            // object within the control's
            // content.
            foreach(RunProperties properties in content.Descendants<RunProperties>().ToList())
            {
                // Create new font size
                // object
                FontSize fontSize = new FontSize();
                fontSize.Val = new StringValue(size.ToString());

                // Assign font size
                properties.FontSize = fontSize;
            }
        }

        /**
        \brief
            Sets the given content control's
            bold property.

            Finds content control using tag.

        \param tag
            Tag to use to find content control
        
        \param bold
            Whether content should be bold or not
        */
        public void SetContentBoldAtTag(
            string tag,
            bool bold)
        {
            // Get element with tag
            SdtElement element = this.ContentControlAtTag(tag);

            // Make sure element exists
            if(element == null)
            {
                // It doesn't.
                // Return.
                return;
            }

            // Set the element's bold
            // property
            this.SetContentBold(element, bold);
        }

        /**
        \brief
            Set's the given content control's
            bold property.

        \param element
            Content control
        
        \param bold
            Whether content should be bold or not
        */
        public void SetContentBold(
            SdtElement element,
            bool bold)
        {
            // Make sure control has properties
            // It does not. Let's add them.
            OpenXmlElement content = this.GetControlContentBlock(element);

            // Iterate through each run property
            // contained within the content
            foreach(RunProperties properties in content.Descendants<RunProperties>())
            {
                // Make the content bold
                // if necessary.
                if (bold)
                {
                    properties.Append(new Bold());
                    return;
                }

                // Content should not be bold.
                // Make sure it is not.
                properties.Bold = null;
                return;
            }
        }

        /**
        \brief
            Sets a content control's highlight
            properties.

            Finds the content control using a tag.

        \param tag
            Tag to use to find content control

        \param highlight
            Determines whether or not content control
            should be highlighted

        \param color
            Highlight color to use. Yellow by default.
        */
        public void SetContentHighlightAtTag(
            string tag,
            bool highlight,
            HighlightColorValues color = HighlightColorValues.Yellow)
        {
            // Get control with given tag
            SdtElement control = this.ContentControlAtTag(tag);

            // Make sure control exists
            if(control == null)
            {
                // It does not. 
                // Return.
                return;
            }

            // Highlight control
            this.SetContentHighlight(control, highlight, color);
        }

        /**
        \brief
            Sets a content control's highlight
            properties.

        \param element
            Content control element to highlight
        
        \param highlight
            Boolean value determining whether or
            not to highlight content

        \param color
            Highlight color to use. Yellow by default.
        */
        public void SetContentHighlight(
            SdtElement element,
            bool highlight,
            HighlightColorValues color = HighlightColorValues.Yellow)
        {
            // Get control with given tag
            SdtElement control = element;

            // Make sure control has properties
            // It does not. Let's add them.
            OpenXmlElement content = this.GetControlContentBlock(control);

            foreach(RunProperties properties in content.Descendants<RunProperties>())
            {
                // Create highlight object
                Highlight hl = null;

                // Assign its properties if
                // we should be highlighting
                // the content
                if (highlight == true)
                {
                    hl = new Highlight();
                    hl.Val = color;
                }

                // Assign highlight
                properties.Highlight = hl;
            }
        }

        /**
        \brief
            Replaces the content of the content
            control(s) with the given tag.

        \param tag
            Tag to find content control with

        \param newContent
            New content for content control
        */
        public void ReplaceContentAtTag(string tag, string newContent)
        {
            // Refers to the main document portion of
            // the NAIC document
            Document naicDoc = this.wordprocessingDocument.MainDocumentPart.Document;

            // Iterate through each SDT element in
            // the document
            foreach(SdtElement element in naicDoc.Descendants<SdtElement>().ToList())
            {
                // Get each <w:tag ... /> element in the
                // SDT element. There should only be one, 
                // but iterate through all of them just in
                // case.
                foreach(Tag contentTag in element.Descendants<Tag>().ToList())
                {
                    // Check to see if the tag value matches
                    // the tag parameter.
                    if (contentTag.Val.Value.Equals(tag) != true)
                    {
                        // It does not.
                        // Continue looping.

                        continue;
                    }

                    // Found a match! Now replace its content
                    // Get content contained within the content
                    // control
                    OpenXmlElement contentElement;

                    // Get content
                    contentElement = this.GetControlContentBlock(element);

                    // No content, nothing to replace
                    if(contentElement == null)
                    {
                        // Continue looping
                        continue;
                    }

                    // Replace text for first run
                    Run firstRun = contentElement.Descendants<Run>().First();
                    firstRun.GetFirstChild<Text>().Text = newContent;

                    // Repeat while there are more than
                    // one elements in the sdtContentBlock
                    while(contentElement.Descendants<Run>().ToList().First() != contentElement.Descendants<Run>().ToList().Last())
                    {
                        // Get the last Run element
                        Run lastElement = contentElement.Descendants<Run>().Last();

                        // Remove it
                        lastElement.Parent.RemoveChild(lastElement);
                        
                    }
                }
            }
        }
    }
}
