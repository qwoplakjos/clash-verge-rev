import type {
  DraggableAttributes,
  DraggableSyntheticListeners,
} from "@dnd-kit/core";
import {
  alpha,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
} from "@mui/material";
import type { CSSProperties, ReactNode } from "react";
import { useMatch, useNavigate, useResolvedPath } from "react-router";

import { useVerge } from "@/hooks/use-verge";

interface SortableProps {
  setNodeRef?: (element: HTMLElement | null) => void;
  attributes?: DraggableAttributes;
  listeners?: DraggableSyntheticListeners;
  style?: CSSProperties;
  isDragging?: boolean;
  disabled?: boolean;
}

interface Props {
  to: string;
  children: string;
  icon: ReactNode[];
  sortable?: SortableProps;
}
export const LayoutItem = (props: Props) => {
  const { to, children, icon, sortable } = props;
  const { verge } = useVerge();
  const { menu_icon } = verge ?? {};
  const navCollapsed = verge?.collapse_navbar ?? false;
  const resolved = useResolvedPath(to);
  const match = useMatch({ path: resolved.pathname, end: true });
  const navigate = useNavigate();

  const effectiveMenuIcon =
    navCollapsed && menu_icon === "disable" ? "monochrome" : menu_icon;

  const { setNodeRef, attributes, listeners, style, isDragging, disabled } =
    sortable ?? {};

  const draggable = Boolean(sortable) && !disabled;
  const dragHandleProps = draggable
    ? { ...(attributes ?? {}), ...(listeners ?? {}) }
    : undefined;

  return (
    <ListItem
      ref={setNodeRef}
      style={style}
      sx={[
        { py: 0, px: 0, width: "auto", maxWidth: "none", mx: 0 },
        isDragging ? { opacity: 0.78 } : {},
      ]}
    >
      <ListItemButton
        selected={!!match}
        {...(dragHandleProps ?? {})}
        sx={[
          {
            borderRadius: 999,
            minHeight: 42,
            px: 1.25,
            gap: 0.75,
            border: "1px solid",
            borderColor: "transparent",
            cursor: draggable ? "grab" : "pointer",
            "&:active": draggable ? { cursor: "grabbing" } : {},
            "& .MuiListItemText-primary": {
              color: "text.primary",
              fontWeight: 600,
              fontSize: 13,
            },
          },
          ({ palette: { mode, primary } }) => {
            const bgcolor =
              mode === "light"
                ? alpha(primary.main, 0.15)
                : alpha(primary.main, 0.35);
            const color = mode === "light" ? "#1f1f1f" : "#ffffff";
            return {
              backgroundColor: alpha(
                primary.main,
                mode === "light" ? 0.03 : 0.08,
              ),
              "&:hover": {
                backgroundColor: alpha(
                  primary.main,
                  mode === "light" ? 0.08 : 0.16,
                ),
              },
              "&.Mui-selected": {
                bgcolor,
                borderColor: alpha(primary.main, 0.4),
              },
              "&.Mui-selected:hover": { bgcolor },
              "&.Mui-selected .MuiListItemText-primary": { color },
            };
          },
        ]}
        title={navCollapsed ? children : undefined}
        aria-label={navCollapsed ? children : undefined}
        onClick={() => navigate(to)}
      >
        {(effectiveMenuIcon === "monochrome" || !effectiveMenuIcon) && (
          <ListItemIcon
            sx={{
              color: "text.primary",
              minWidth: "auto",
              margin: 0,
              cursor: draggable ? "grab" : "inherit",
            }}
          >
            {icon[0]}
          </ListItemIcon>
        )}
        {effectiveMenuIcon === "colorful" && (
          <ListItemIcon
            sx={{
              minWidth: "auto",
              margin: 0,
              cursor: draggable ? "grab" : "inherit",
            }}
          >
            {icon[1]}
          </ListItemIcon>
        )}
        <ListItemText
          sx={{
            textAlign: "left",
            m: 0,
          }}
          primary={children}
        />
      </ListItemButton>
    </ListItem>
  );
};
