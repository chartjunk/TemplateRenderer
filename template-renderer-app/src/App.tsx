/// <reference path="../node_modules/jquery-resizable-dom/src/jquery-resizable.d.ts" />
import React, { useState, useEffect } from 'react';
import AppBar from '@material-ui/core/AppBar';
import CssBaseline from '@material-ui/core/CssBaseline';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Editor from 'react-simple-code-editor';
import Highlight, { defaultProps } from 'prism-react-renderer';
import theme from 'prism-react-renderer/themes/nightOwl';
import { makeStyles, Grid } from '@material-ui/core';
import $ from 'jquery';
import 'jquery-resizable-dom';

const useStyles = makeStyles({
  editor: {
    boxSizing: 'border-box',
    fontFamily: '"Dank Mono", "Fira Code", monospace',
    height: '100%',
    ...(theme.plain as any),
  },
  panelContainer: {
    display: 'flex',
    flexDirection: 'row',
    border: '1px solid silver',
    overflow: 'hidden',
    xtouchAction: 'none',
  },
  panelLeft: {
    flex: '0 0 auto',
    width: '45%',
    minHeight: '200px',
    minWidth: '150px',
    whiteSpace: 'nowrap',
    ...(theme.plain as any),
  },
  splitterLeftRight: {},
  panelRight: {},
});

export default function Album() {
  const [code, setCode] = useState('var a = 1;');
  const [entry, setEntry] = useState('var a = 1;');
  const classes = useStyles();
  useEffect(() => {
    $('.panel-left').resizable({
      handleSelector: '.splitter-left-right',
      resizeHeight: false,
    });
  });
  const highlight = (code: string) => (
    <Highlight {...defaultProps} code={code} theme={theme} language="jsx">
      {({ className, style, tokens, getLineProps, getTokenProps }) => (
        <>
          {tokens.map((line, i) => (
            <div {...getLineProps({ line, key: i })}>
              {line.map((token, key) => (
                <span {...getTokenProps({ token, key })} />
              ))}
            </div>
          ))}
        </>
      )}
    </Highlight>
  );

  return (
    <React.Fragment>
      <CssBaseline />
      <AppBar position="relative">
        <Toolbar>
          {/* <CameraIcon className={classes.icon} /> */}
          <Typography variant="h6" color="inherit" noWrap>
            TemplateRenderer
          </Typography>
        </Toolbar>
      </AppBar>
      <main>
        <Grid container className={classes.panelContainer} direction="row">
          <Grid item className={classes.panelLeft + ' panel-left'}>
            <Editor
              value={code}
              onValueChange={setCode}
              highlight={highlight}
              className={classes.editor}
              padding={10}
            />
          </Grid>
          <Grid
            item
            className={classes.splitterLeftRight + ' splitter-left-right'}
            style={{ width: '50px', background: 'green' }}
          />
          <Grid className={classes.panelRight + ' panel-right'}>LUL</Grid>
        </Grid>
      </main>
      {/* Footer */}
      {/* <footer className={classes.footer}>
        <Typography variant="h6" align="center" gutterBottom>
          Footer
        </Typography>
        <Typography
          variant="subtitle1"
          align="center"
          color="textSecondary"
          component="p"
        >
          Something here to give the footer a purpose!
        </Typography>
        <Copyright />
      </footer> */}
      {/* End footer */}
    </React.Fragment>
  );
}
